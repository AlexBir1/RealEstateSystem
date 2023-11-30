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
    public class AgreementService : IAgreementService
    {
        private readonly IDBRepository _dBRepository;
        private readonly IMapper _mapper;

        public AgreementService(IDBRepository dBRepository, IMapper mapper)
        {
            _dBRepository = dBRepository;
            _mapper = mapper;
        }

        public async Task<ResponseWrapper<AgreementModel>> DeleteAsync(string id)
        {
            try
            {
                var result = await _dBRepository.AgreementRepo.DeleteAsync(id);

                if (result.Data == null)
                    return new ResponseWrapper<AgreementModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<AgreementModel>(commitResult.Errors);

                var outputModel = _mapper.Map<AgreementModel>(result.Data);
                return new ResponseWrapper<AgreementModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<AgreementModel>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<AgreementModel>>> GetAllAsync()
        {
            try
            {
                var result = await _dBRepository.AgreementRepo.GetAllAsync();

                if (result.Data == null)
                    return new ResponseWrapper<IEnumerable<AgreementModel>>(result.Errors);

                var outputModels = _mapper.Map<IEnumerable<AgreementModel>>(result.Data);
                return new ResponseWrapper<IEnumerable<AgreementModel>>(outputModels);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<IEnumerable<AgreementModel>>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<AgreementModel>>> GetAllByAccountIdAsync(string accountId)
        {
            try
            {
                var result = await _dBRepository.AgreementRepo.GetAllByAccountIdAsync(accountId);

                if (result.Data == null)
                    return new ResponseWrapper<IEnumerable<AgreementModel>>(result.Errors);

                var outputModels = _mapper.Map<IEnumerable<AgreementModel>>(result.Data);
                return new ResponseWrapper<IEnumerable<AgreementModel>>(outputModels);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<IEnumerable<AgreementModel>>(errors);
            }
        }

        public async Task<ResponseWrapper<AgreementModel>> GetByIdAsync(string apartmentId)
        {
            try
            {
                var result = await _dBRepository.AgreementRepo.GetByIdAsync(apartmentId);

                if (result.Data == null)
                    return new ResponseWrapper<AgreementModel>(result.Errors);

                var outputModel = _mapper.Map<AgreementModel>(result.Data);
                return new ResponseWrapper<AgreementModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<AgreementModel>(errors);
            }
        }

        public async Task<ResponseWrapper<AgreementModel>> InsertAsync(AgreementModel model)
        {
            try
            {
                var result = await _dBRepository.AgreementRepo.InsertAsync(_mapper.Map<Agreement>(model));

                if (result.Data == null)
                    return new ResponseWrapper<AgreementModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<AgreementModel>(commitResult.Errors);

                var outputModel = _mapper.Map<AgreementModel>(result.Data);
                return new ResponseWrapper<AgreementModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<AgreementModel>(errors);
            }
        }

        public async Task<ResponseWrapper<AgreementModel>> UpdateAsync(string id, AgreementModel model)
        {
            try
            {
                var result = await _dBRepository.AgreementRepo.UpdateAsync(id, _mapper.Map<Agreement>(model));

                if (result.Data == null)
                    return new ResponseWrapper<AgreementModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<AgreementModel>(commitResult.Errors);

                var outputModel = _mapper.Map<AgreementModel>(result.Data);
                return new ResponseWrapper<AgreementModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<AgreementModel>(errors);
            }
        }
    }
}
