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
    public class CallService : ICallService
    {
        private readonly IDBRepository _dbRepository;
        private readonly IMapper _mapper;

        public CallService(IDBRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<ResponseWrapper<CallModel>> DeleteAsync(string id)
        {
            try
            {
                var result = await _dbRepository.CallRepo.DeleteAsync(id);

                if(result.Data == null)
                    return new ResponseWrapper<CallModel>(result.Errors);

                var commitResult = await _dbRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<CallModel>(commitResult.Errors);

                var outputModel = _mapper.Map<CallModel>(result.Data);
                return new ResponseWrapper<CallModel>(outputModel);
            }
            catch(Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<CallModel>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<CallModel>>> GetAllAsync()
        {
            try
            {
                var result = await _dbRepository.CallRepo.GetAllAsync();

                if (result.Data.Count() == 0)
                    return new ResponseWrapper<IEnumerable<CallModel>>(result.Errors);

                var outputModel = _mapper.Map<IEnumerable<CallModel>>(result.Data);
                return new ResponseWrapper<IEnumerable<CallModel>>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<IEnumerable<CallModel>>(errors);
            }
        }

        public async Task<ResponseWrapper<CallModel>> InsertAsync(RequestCallModel model)
        {
            try
            {
                var entity = _mapper.Map<Call>(model);
                var result = await _dbRepository.CallRepo.InsertAsync(entity);

                if (result.Data == null)
                    return new ResponseWrapper<CallModel>(result.Errors);

                var commitResult = await _dbRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<CallModel>(commitResult.Errors);

                var outputModel = _mapper.Map<CallModel>(result.Data);
                return new ResponseWrapper<CallModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<CallModel>(errors);
            }
        }

        public async Task<ResponseWrapper<CallModel>> UpdateAsync(string id, CallModel model)
        {
            try
            {
                var entity = _mapper.Map<Call>(model);
                var result = await _dbRepository.CallRepo.UpdateAsync(id, entity);

                if (result.Data == null)
                    return new ResponseWrapper<CallModel>(result.Errors);

                var commitResult = await _dbRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<CallModel>(commitResult.Errors);

                var outputModel = _mapper.Map<CallModel>(result.Data);
                return new ResponseWrapper<CallModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<CallModel>(errors);
            }
        }
    }
}
