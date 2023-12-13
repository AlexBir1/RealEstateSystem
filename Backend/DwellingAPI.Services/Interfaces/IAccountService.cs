
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
        Task<ResponseWrapper<AccountModel>> LogOutAsync();
        Task<ResponseWrapper<AccountModel>> LogInAsync(LogInModel model);
        Task<ResponseWrapper<AccountModel>> ChangePasswordAsync(string accountId, ChangePasswordModel model);
        Task<ResponseWrapper<AccountModel>> InsertAsync(SignUpModel model);
        Task<ResponseWrapper<AccountModel>> GetByIdAsync(string accountId);
        Task<ResponseWrapper<string>> SetRoleAsync(string accountId, string newRole);
        Task<ResponseWrapper<string>> GetRoleAsync(string accountId);
        Task<ResponseWrapper<IEnumerable<AccountModel>>> GetAllAsync();
    }
}
