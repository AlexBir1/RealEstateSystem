
using DwellingAPI.DAL.Entities;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.Interfaces
{
    public interface IAccountService : IService<AccountModel>
    {
        public Task<ResponseWrapper<AccountModel>> LogOutAsync();
        public Task<ResponseWrapper<AccountModel>> LogInAsync(LogInModel model);
        public Task<ResponseWrapper<AccountModel>> ChangePasswordAsync(string accountId, ChangePasswordModel model);
        public Task<ResponseWrapper<AccountModel>> InsertAsync(SignUpModel model);
        public Task<ResponseWrapper<AccountModel>> GetByIdAsync(string accountId);
        public Task<ResponseWrapper<string>> SetRoleAsync(string accountId, string newRole);
        public Task<ResponseWrapper<string>> GetRoleAsync(string accountId);
    }
}
