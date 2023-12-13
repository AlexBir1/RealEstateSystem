using DwellingAPI.DAL.Entities;
using DwellingAPI.ResponseWrapper.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        public Task<Account> LogOutAsync();
        public Task<Account> LogInAsync(string userIdentifier, string password);
        public Task<Account> ChangePasswordAsync(string id, string oldPassword, string newPassword);
        public Task<Account> SetPasswordAsync(string id, string newPassword);
        public Task<string> SetRoleAsync(string accountId, string newRole);
        public Task<string> GetRoleAsync(string accountId);
        Task<Account> GetByUsernameAsync(string username);
    }
}
