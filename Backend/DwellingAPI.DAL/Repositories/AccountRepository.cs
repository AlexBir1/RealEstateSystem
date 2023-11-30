using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Extensions;
using DwellingAPI.DAL.Interfaces;
using DwellingAPI.ResponseWrapper.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;

        public AccountRepository(UserManager<Account> userManager, SignInManager<Account> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ResponseWrapper<Account>> ChangePasswordAsync(string id, string oldPassword, string newPassword)
        {
            try
            {
                var account = await _userManager.FindByIdAsync(id);

                if (account == null)
                    return new ResponseWrapper<Account>(new List<string> { new string("Account is not found") });

                var result = await _userManager.ChangePasswordAsync(account, oldPassword, newPassword);

                if (result.Succeeded)
                {
                    return new ResponseWrapper<Account>(account);
                }

                return new ResponseWrapper<Account>(result.Errors.Select(x => x.Description));
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Account>(errors);
            }
        }

        public async Task<ResponseWrapper<Account>> DeleteAsync(string id)
        {
            try
            {
                var account = await _userManager.FindByIdAsync(id);

                if (account != null)
                {
                    var result = await _userManager.DeleteAsync(account);

                    if (result.Succeeded)
                        return new ResponseWrapper<Account>(account);

                    return new ResponseWrapper<Account>(result.Errors.Select(x => x.Description));
                }

                var errors = new List<string>()
                {
                    new string("Account is not found")
                };

                return new ResponseWrapper<Account>(errors);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Account>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<Account>>> GetAllAsync()
        {
            try
            {
                var accounts = await _userManager.Users.ToListAsync();
                if (accounts.Count() == 0)
                {
                    var errors = new List<string>()
                    {
                        new string("List is empty")
                    };

                    return new ResponseWrapper<IEnumerable<Account>>(errors);
                }
                return new ResponseWrapper<IEnumerable<Account>>(accounts);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<IEnumerable<Account>>(errors);
            }
        }

        public async Task<ResponseWrapper<Account>> GetByIdAsync(string id)
        {
            try
            {
                var account = await _userManager.FindByIdAsync(id);

                if (account != null)
                    return new ResponseWrapper<Account>(account);

                var errors = new List<string>()
                {
                    new string("Account is not found")
                };

                return new ResponseWrapper<Account>(errors);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Account>(errors);
            }
        }

        public async Task<ResponseWrapper<string>> GetRoleAsync(string accountId)
        {
            try
            {
                var account = await _userManager.FindByIdAsync(accountId);

                if(account == null)
                    return new ResponseWrapper<string>(new List<string> { new string("Account doesn`t exist") });

                var accountRoles = await _userManager.GetRolesAsync(account);

                if(accountRoles.Count == 0)
                    return new ResponseWrapper<string>(new List<string> { new string("No role")});

                return new ResponseWrapper<string>(accountRoles.First());
            }
            catch(Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<string>(errors);
            }
        }

        public async Task<ResponseWrapper<Account>> InsertAsync(Account entity)
        {
            try
            {
                var result = await _userManager.CreateAsync(entity);

                if (!result.Succeeded)
                    return new ResponseWrapper<Account>(result.Errors.Select(x => x.Description));

                entity.CreationDate = DateTime.Now;
                entity.LastlyUpdatedDate = DateTime.Now;
                await _userManager.UpdateAsync(entity);

                return new ResponseWrapper<Account>(entity);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Account>(errors);
            }
        }

        public async Task<ResponseWrapper<Account>> LogInAsync(string userIdentifier, string password)
        {
            var account = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == userIdentifier || x.Email == userIdentifier || x.PhoneNumber == userIdentifier);

            if (account == null)
            {
                var error = new List<string>()
                {
                    new string("Account is not found"),
                };
                return new ResponseWrapper<Account>(error);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(account, password, false);

            if (result.Succeeded)
                return new ResponseWrapper<Account>(account);

            var errors = new List<string>()
            {
                new string("Invalid login or password"),
            };
            return new ResponseWrapper<Account>(errors);
        }

        public async Task<ResponseWrapper<Account>> LogOutAsync()
        {
            try
            {
                var accountId = _signInManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return new ResponseWrapper<Account>(await _userManager.FindByIdAsync(accountId));
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Account>(errors);
            }
        }

        public async Task<ResponseWrapper<Account>> SetPasswordAsync(string id, string newPassword)
        {
            try
            {
                var account = await _userManager.FindByIdAsync(id);

                if (account == null)
                {
                    var error = new List<string>()
                        {
                            new string("Account is not found"),
                        };

                    return new ResponseWrapper<Account>(error);
                }

                var result = await _userManager.AddPasswordAsync(account, newPassword);

                if (!result.Succeeded)
                    return new ResponseWrapper<Account>(result.Errors.Select(x => x.Description));

                account.LastlyUpdatedDate = DateTime.Now;
                await _userManager.UpdateAsync(account);

                return new ResponseWrapper<Account>(account);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Account>(errors);
            }
        }

        public async Task<ResponseWrapper<string>> SetRoleAsync(string accountId, string newRole)
        {
            try
            {
                var account = await _userManager.FindByIdAsync(accountId);

                if (account == null)
                    return new ResponseWrapper<string>(new List<string> { new string("Account doesn`t exist") });

                if (await _userManager.IsInRoleAsync(account, newRole) == true)
                    return new ResponseWrapper<string>(new List<string> { new string("Account is currently in this role") });
                
                var result = await _userManager.AddToRoleAsync(account, newRole);

                account.LastlyUpdatedDate = DateTime.Now;
                await _userManager.UpdateAsync(account);

                if (!result.Succeeded)
                    return new ResponseWrapper<string>(result.Errors.Select(x=>x.Description));

                return new ResponseWrapper<string>(newRole);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<string>(errors);
            }
        }

        public async Task<ResponseWrapper<Account>> UpdateAsync(string id, Account entity)
        {
            try
            {
                var account = await _userManager.FindByIdAsync(id);

                if (account != null)
                {
                    if (entity.UserName == account.UserName
                    && entity.Email == account.Email
                    && entity.FullName == account.FullName
                    && entity.PhoneNumber == account.PhoneNumber)
                    {
                        var error = new List<string>()
                        {
                            new string("Nothing to change"),
                        };

                        return new ResponseWrapper<Account>(error);
                    }
                    else
                    {
                        if (entity.UserName != account.UserName)
                            await _userManager.SetUserNameAsync(account, entity.UserName);

                        if (entity.Email != account.Email)
                            await _userManager.SetEmailAsync(account, entity.Email);

                        if (entity.FullName != account.FullName)
                            await _userManager.ChangeFullnameAsync(account, entity.FullName);

                        if (entity.PhoneNumber != account.PhoneNumber)
                            await _userManager.SetPhoneNumberAsync(account, entity.PhoneNumber);

                        account.LastlyUpdatedDate = DateTime.Now;

                        await _userManager.UpdateAsync(account);

                        account = await _userManager.FindByIdAsync(id);

                        return new ResponseWrapper<Account>(account);
                    }
                }

                var errors = new List<string>()
                {
                    new string("Account does not exist"),
                };

                return new ResponseWrapper<Account>(errors);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Account>(errors);
            }
        }
    }
}
