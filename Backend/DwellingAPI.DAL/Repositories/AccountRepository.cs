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
using DwellingAPI.DAL.Exceptions;

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

        public async Task<Account> ChangePasswordAsync(string id, string oldPassword, string newPassword)
        {
            var account = await _userManager.FindByIdAsync(id);

            if (account == null)
                throw new Exception("Account does not exist");

            var result = await _userManager.ChangePasswordAsync(account, oldPassword, newPassword);

            if (result.Succeeded)
                return account;
            else
                throw new OperationFailedException(result.Errors.Select(x => x.Description));
        }

        public async Task<Account> DeleteAsync(string id)
        {
            var account = await _userManager.FindByIdAsync(id);

            if (account == null)
                throw new OperationFailedException("Account does not exist");

            var result = await _userManager.DeleteAsync(account);

            if (result.Succeeded)
                return account;
            else
                throw new OperationFailedException(result.Errors.Select(x => x.Description));
        }



        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            var accounts = await _userManager.Users.ToListAsync();

            if (accounts.Count() == 0)
                throw new Exception("List is empty");

            return accounts;
        }

        public async Task<Account> GetByIdAsync(string id)
        {
            var account = await _userManager.FindByIdAsync(id);

            if (account == null)
                throw new OperationFailedException("Account is not found");
            return account;
        }

        public async Task<Account> GetByUsernameAsync(string username)
        {
            var account = await _userManager.FindByNameAsync(username);

            if (account == null)
                throw new OperationFailedException("Account is not found");
            return account;
        }

        public async Task<string> GetRoleAsync(string accountId)
        {
            var account = await _userManager.FindByIdAsync(accountId);

            if (account == null)
                throw new OperationFailedException("Account is not found");

            var accountRoles = await _userManager.GetRolesAsync(account);

            return accountRoles.First();
        }

        public async Task<Account> InsertAsync(Account entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.LastlyUpdatedDate = DateTime.Now;

            var result = await _userManager.CreateAsync(entity);

            if (!result.Succeeded)
                throw new OperationFailedException(result.Errors.Select(x => x.Description));
            else
                return entity;
        }

        public async Task<Account> LogInAsync(string userIdentifier, string password)
        {
            var account = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == userIdentifier || x.Email == userIdentifier || x.PhoneNumber == userIdentifier);

            if (account == null)
                throw new OperationFailedException("Invalid login or password");

            var result = await _signInManager.CheckPasswordSignInAsync(account, password, false);

            if (!result.Succeeded)
                throw new OperationFailedException("Invalid login or password");
            else
                return account;
        }

        public async Task<Account> LogOutAsync()
        {

            var accountId = _signInManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(accountId))
                throw new OperationFailedException("Cannot log out. User is not logged in.");
            else
                return new Account { Id = accountId };
        }

        public async Task<Account> SetPasswordAsync(string id, string newPassword)
        {
            var account = await _userManager.FindByIdAsync(id);

            if (account == null)
                throw new OperationFailedException("Account is not found");

            var result = await _userManager.AddPasswordAsync(account, newPassword);

            if (!result.Succeeded)
                throw new OperationFailedException(result.Errors.Select(x => x.Description));
            else
                return account;
        }

        public async Task<string> SetRoleAsync(string accountId, string newRole)
        {
            var account = await _userManager.FindByIdAsync(accountId);

            if (account == null)
                throw new OperationFailedException("Account is not found");

            if (await _userManager.IsInRoleAsync(account, newRole) == true)
                throw new OperationFailedException("Account is currently in this role");

            var result = await _userManager.AddToRoleAsync(account, newRole);

            if (!result.Succeeded)
                throw new OperationFailedException(result.Errors.Select(x => x.Description));
            else
                return newRole;

        }

        public async Task<Account> UpdateAsync(string id, Account entity)
        {
            var account = await _userManager.FindByIdAsync(id);

            if (account != null)
            {
                if (entity.UserName == account.UserName
                && entity.Email == account.Email
                && entity.FullName == account.FullName
                && entity.PhoneNumber == account.PhoneNumber)
                    throw new OperationFailedException("Nothing to change");
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

                    return account;
                }
            }
            else
                throw new OperationFailedException("Account is not found");
        }
    }
}
