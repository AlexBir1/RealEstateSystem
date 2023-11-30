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
            try
            {
                var result = await _dBRepository.AccountRepo.ChangePasswordAsync(accountId, model.OldPassword, model.NewPassword);
                if (result.Data == null)
                    return new ResponseWrapper<AccountModel>(result.Errors);

                var outputModel = _mapper.Map<AccountModel>(result.Data);
                return new ResponseWrapper<AccountModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<AccountModel>(errors);
            }
        }

        public async Task<ResponseWrapper<AccountModel>> DeleteAsync(string id)
        {
            try
            {
                var result = await _dBRepository.AccountRepo.DeleteAsync(id);
                if (result.Data == null)
                    return new ResponseWrapper<AccountModel>(result.Errors);

                var outputModel = _mapper.Map<AccountModel>(result.Data);
                return new ResponseWrapper<AccountModel>(outputModel);
            }
            catch(Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<AccountModel>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<AccountModel>>> GetAllAsync()
        {
            try
            {
                var result = await _dBRepository.AccountRepo.GetAllAsync();
                if (result.Data == null)
                    return new ResponseWrapper<IEnumerable<AccountModel>>(result.Errors);

                var outputModel = _mapper.Map<IEnumerable<AccountModel>>(result.Data);
                return new ResponseWrapper<IEnumerable<AccountModel>>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<IEnumerable<AccountModel>>(errors);
            }
        }

        public async Task<ResponseWrapper<AccountModel>> GetByIdAsync(string accountId)
        {
            try
            {
                var result = await _dBRepository.AccountRepo.GetByIdAsync(accountId);
                if (result.Data == null)
                    return new ResponseWrapper<AccountModel>(result.Errors);

                var outputModel = _mapper.Map<AccountModel>(result.Data);

                return new ResponseWrapper<AccountModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<AccountModel>(errors);
            }
        }

        public async Task<ResponseWrapper<string>> GetRoleAsync(string accountId)
        {
            try
            {
                return await _dBRepository.AccountRepo.GetRoleAsync(accountId);
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

        public async Task<ResponseWrapper<AccountModel>> InsertAsync(SignUpModel model)
        {
            try
            {
                var entity = _mapper.Map<Account>(model);

                var registerResult = await _dBRepository.AccountRepo.InsertAsync(entity);
                if (registerResult.Data == null)
                    return new ResponseWrapper<AccountModel>(registerResult.Errors);

                var setPasswordResult = await _dBRepository.AccountRepo.SetPasswordAsync(registerResult.Data.Id, model.PasswordConfirm);
                if (setPasswordResult.Data == null)
                    return new ResponseWrapper<AccountModel>(setPasswordResult.Errors);

                var outputModel = _mapper.Map<AccountModel>(entity);
                return new ResponseWrapper<AccountModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<AccountModel>(errors);
            }
        }

        public async Task<ResponseWrapper<AccountModel>> LogInAsync(LogInModel model)
        {
            try
            {
                var result = await _dBRepository.AccountRepo.LogInAsync(model.UserIdentifier, model.Password);
                if (result.Data == null)
                    return new ResponseWrapper<AccountModel>(result.Errors);

                var outputModel = _mapper.Map<AccountModel>(result.Data);
                return new ResponseWrapper<AccountModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<AccountModel>(errors);
            }
        }

        public async Task<ResponseWrapper<AccountModel>> LogOutAsync()
        {
            try
            {
                var result = await _dBRepository.AccountRepo.LogOutAsync();
                if (result.Data == null)
                    return new ResponseWrapper<AccountModel>(result.Errors);

                var outputModel = _mapper.Map<AccountModel>(result.Data);
                return new ResponseWrapper<AccountModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<AccountModel>(errors);
            }
        }

        public async Task<ResponseWrapper<string>> SetRoleAsync(string accountId, string newRole)
        {
            try
            {
                return await _dBRepository.AccountRepo.SetRoleAsync(accountId, newRole);
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

        public async Task<ResponseWrapper<AccountModel>> UpdateAsync(string id, AccountModel model)
        {
            try
            {
                var entity = _mapper.Map<Account>(model);
                var result = await _dBRepository.AccountRepo.UpdateAsync(id, entity);
                if (result.Data == null)
                    return new ResponseWrapper<AccountModel>(result.Errors);

                var outputModel = _mapper.Map<AccountModel>(result.Data);
                return new ResponseWrapper<AccountModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<AccountModel>(errors);
            }
        }
    }
}
