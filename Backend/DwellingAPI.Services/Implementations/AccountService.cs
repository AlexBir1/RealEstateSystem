using AutoMapper;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.UOW;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Services.Interfaces;
using DwellingAPI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IDBRepository _dBRepository;
        private readonly IMapper _mapper;

        public AccountService(IDBRepository dBRepository, IMapper mapper)
        {
            _dBRepository = dBRepository;
            _mapper = mapper;
        }

        public async Task<ResponseWrapper<AccountModel>> ChangePasswordAsync(string accountId, ChangePasswordModel model)
        {
            return new ResponseWrapper<AccountModel>(
                _mapper.Map<AccountModel>(
                    await _dBRepository.AccountRepo.ChangePasswordAsync(accountId, model.OldPassword, model.NewPassword)
                    )
                );
        }

        public async Task<ResponseWrapper<AccountModel>> DeleteAsync(string id)
        {
            return new ResponseWrapper<AccountModel>(
                _mapper.Map<AccountModel>(
                    await _dBRepository.AccountRepo.DeleteAsync(id)
                    )
                );
        }

        public async Task<ResponseWrapper<IEnumerable<AccountModel>>> GetAllAsync()
        {
            return new ResponseWrapper<IEnumerable<AccountModel>>(
                _mapper.Map<IEnumerable<AccountModel>>(
                    await _dBRepository.AccountRepo.GetAllAsync()
                    )
                );
        }

        public async Task<ResponseWrapper<AccountModel>> GetByIdAsync(string accountId)
        {
            return new ResponseWrapper<AccountModel>(
                _mapper.Map<AccountModel>(
                    await _dBRepository.AccountRepo.GetByIdAsync(accountId)
                    )
                );
        }

        public async Task<ResponseWrapper<string>> GetRoleAsync(string accountId)
        {
            return new ResponseWrapper<string>(await _dBRepository.AccountRepo.GetRoleAsync(accountId));
        }

        public async Task<ResponseWrapper<AccountModel>> InsertAsync(SignUpModel model)
        {
            if(await _dBRepository.AccountRepo.GetByUsernameAsync(model.Username) != null)
            {
                throw new Exception("Account does not exist");
            }
                

            var registerResult = await _dBRepository.AccountRepo.InsertAsync(_mapper.Map<Account>(model));

            return new ResponseWrapper<AccountModel>(
                _mapper.Map<AccountModel>(
                    await _dBRepository.AccountRepo.SetPasswordAsync(registerResult.Id, model.PasswordConfirm)
                    )
                );
        }

        public async Task<ResponseWrapper<AccountModel>> LogInAsync(LogInModel model)
        {
            return new ResponseWrapper<AccountModel>(
                _mapper.Map<AccountModel>(
                    await _dBRepository.AccountRepo.LogInAsync(model.UserIdentifier, model.Password)
                    )
                );
        }

        public async Task<ResponseWrapper<AccountModel>> LogOutAsync()
        {
            return new ResponseWrapper<AccountModel>(
             _mapper.Map<AccountModel>(
                 await _dBRepository.AccountRepo.LogOutAsync()
                 )
             );
        }

        public async Task<ResponseWrapper<string>> SetRoleAsync(string accountId, string newRole)
        {
            return new ResponseWrapper<string>(await _dBRepository.AccountRepo.SetRoleAsync(accountId, newRole));
        }

        public async Task<ResponseWrapper<AccountModel>> UpdateAsync(string id, AccountModel model)
        {
            return new ResponseWrapper<AccountModel>(
                _mapper.Map<AccountModel>(
                    await _dBRepository.AccountRepo.UpdateAsync(id, _mapper.Map<Account>(model))
                    )
                );
        }
    }
}
