using DwellingAPI.DAL.Entities;
using DwellingAPI.ResponseWrapper.Implementation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Extensions
{
    public static class IdentityExtensions
    {
        public static async Task<ResponseWrapper<Account>> ChangeFullnameAsync(this UserManager<Account> target, Account account, string newFullname)
        {
            var user = await target.FindByIdAsync(account.Id);
            if (user != null)
            {
                user.FullName = newFullname;
                var result = await target.UpdateAsync(user);
                if (result.Succeeded)
                    return new ResponseWrapper<Account>(user);
                return new ResponseWrapper<Account>(result.Errors.Select(x => x.Description));
            }
            var errors = new List<string>()
            {
                new string("Account cannot be update because it does not exist"),
            };

            return new ResponseWrapper<Account>(errors);
        }
    }
}
