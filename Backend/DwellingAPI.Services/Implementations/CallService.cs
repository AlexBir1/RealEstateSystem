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
            var result = await _dbRepository.CallRepo.DeleteAsync(id);

            await _dbRepository.CommitAsync();

            return new ResponseWrapper<CallModel>(_mapper.Map<CallModel>(result));
        }

        public async Task<ResponseWrapper<IEnumerable<CallModel>>> GetAllAsync()
        {
            return new ResponseWrapper<IEnumerable<CallModel>>(
                _mapper.Map<IEnumerable<CallModel>>(
                    await _dbRepository.CallRepo.GetAllAsync()
                    )
                );
        }

        public async Task<ResponseWrapper<CallModel>> InsertAsync(RequestCallModel model)
        {
            var result = await _dbRepository.CallRepo.InsertAsync(_mapper.Map<Call>(model));

            await _dbRepository.CommitAsync();

            return new ResponseWrapper<CallModel>(_mapper.Map<CallModel>(result));
        }

        public async Task<ResponseWrapper<CallModel>> UpdateAsync(string id, CallModel model)
        {
            var result = await _dbRepository.CallRepo.UpdateAsync(id, _mapper.Map<Call>(model));

            await _dbRepository.CommitAsync();

            return new ResponseWrapper<CallModel>(_mapper.Map<CallModel>(result));
        }
    }
}
