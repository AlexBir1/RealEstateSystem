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
            var result = await _dBRepository.AgreementRepo.DeleteAsync(id);

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<AgreementModel>(_mapper.Map<AgreementModel>(result));
        }

        public async Task<ResponseWrapper<IEnumerable<AgreementModel>>> GetAllAsync(string accountId = "")
        {
            IEnumerable<Agreement> result;

            if (string.IsNullOrEmpty(accountId))
                result = await _dBRepository.AgreementRepo.GetAllAsync();
            else
                result = await _dBRepository.AgreementRepo.GetAllByAccountIdAsync(accountId);

            return new ResponseWrapper<IEnumerable<AgreementModel>>(_mapper.Map<IEnumerable<AgreementModel>>(result));
        }

        public async Task<ResponseWrapper<AgreementModel>> GetByIdAsync(string apartmentId)
        {
            return new ResponseWrapper<AgreementModel>(
                _mapper.Map<AgreementModel>(
                    await _dBRepository.AgreementRepo.GetByIdAsync(apartmentId)
                    )
                );
        }

        public async Task<ResponseWrapper<AgreementModel>> InsertAsync(AgreementModel model)
        {

            var result = await _dBRepository.AgreementRepo.InsertAsync(_mapper.Map<Agreement>(model));

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<AgreementModel>(_mapper.Map<AgreementModel>(result));

        }

        public async Task<ResponseWrapper<AgreementModel>> UpdateAsync(string id, AgreementModel model)
        {
            var result = await _dBRepository.AgreementRepo.UpdateAsync(id, _mapper.Map<Agreement>(model));

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<AgreementModel>(_mapper.Map<AgreementModel>(result));
        }
    }
}
