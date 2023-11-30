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
        public Task<ResponseWrapper<Account>> LogOutAsync();
        public Task<ResponseWrapper<Account>> LogInAsync(string userIdentifier, string password);
        public Task<ResponseWrapper<Account>> ChangePasswordAsync(string id, string oldPassword, string newPassword);
        public Task<ResponseWrapper<Account>> SetPasswordAsync(string id, string newPassword);
        public Task<ResponseWrapper<string>> SetRoleAsync(string accountId, string newRole);
        public Task<ResponseWrapper<string>> GetRoleAsync(string accountId);
    }
}
