using AutoMapper;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.UOW;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Services.Interfaces;
using DwellingAPI.Shared.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.Implementations
{
    public class ApartmentService : IApartmentService
    {
        private readonly IDBRepository _dBRepository;
        private readonly IMapper _mapper;

        public ApartmentService(IDBRepository dBRepository, IMapper mapper)
        {
            _dBRepository = dBRepository;
            _mapper = mapper;
        }

        public async Task<ResponseWrapper<ApartmentModel>> AddMainPhotoAsync(ApartmentPhotoModel model)
        {
            var result = await _dBRepository.ApartmentRepo.AddMainPhotoAsync(model.ApartmentId.ToString(), model.PhotoFile);

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<ApartmentModel>(_mapper.Map<ApartmentModel>(result));
        }

        public async Task<ResponseWrapper<ApartmentPhotoModel>> AddPhotoAsync(ApartmentPhotoModel model)
        {
            var result = await _dBRepository.ApartmentRepo.AddPhotoAsync(_mapper.Map<ApartmentPhoto>(model), model.PhotoFile);

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<ApartmentPhotoModel>(_mapper.Map<ApartmentPhotoModel>(result));
        }

        public async Task<ResponseWrapper<ApartmentModel>> DeleteAsync(string id)
        {
            var result = await _dBRepository.ApartmentRepo.DeleteAsync(id);

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<ApartmentModel>(_mapper.Map<ApartmentModel>(result));
        }

        public async Task<ResponseWrapper<ApartmentModel>> DeleteMainPhotoAsync(string apartmentId)
        {
            var result = await _dBRepository.ApartmentRepo.DeleteMainPhotoAsync(apartmentId);

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<ApartmentModel>(_mapper.Map<ApartmentModel>(result));
        }

        public async Task<ResponseWrapper<ApartmentPhotoModel>> DeletePhotoAsync(string apartmentId, string photoId)
        {
            var result = await _dBRepository.ApartmentRepo.DeletePhotoAsync(apartmentId, photoId);

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<ApartmentPhotoModel>(_mapper.Map<ApartmentPhotoModel>(result));
        }

        public async Task<ResponseWrapper<IEnumerable<ApartmentModel>>> GetAllAsync()
        {
            return new ResponseWrapper<IEnumerable<ApartmentModel>>(
                _mapper.Map<IEnumerable<ApartmentModel>>(
                    await _dBRepository.ApartmentRepo.GetAllAsync()
                    )
                );
        }

        public async Task<ResponseWrapper<IEnumerable<ApartmentModel>>> GetAllByOrderRequirementsAsync(string orderId)
        {
            return new ResponseWrapper<IEnumerable<ApartmentModel>>(
                _mapper.Map<IEnumerable<ApartmentModel>>(
                    await _dBRepository.ApartmentRepo.GetAllByOrderRequirementsAsync(orderId)
                    )
                );
        }

        public async Task<ResponseWrapper<ApartmentModel>> GetAllPhotosAsync(string apartmentId)
        {
            return new ResponseWrapper<ApartmentModel>(
                _mapper.Map<ApartmentModel>(
                    await _dBRepository.ApartmentRepo.GetAllPhotosAsync(apartmentId)
                    )
                );
        }

        public async Task<ResponseWrapper<ApartmentModel>> GetByIdAsync(string id)
        {
            var result = await _dBRepository.ApartmentRepo.GetByIdAsync(id);

            var outputModel = _mapper.Map<ApartmentModel>(result);

            var outputOrderModels = !result.ApartmentOrders.IsNullOrEmpty() ? _mapper.Map<ICollection<OrderModel>>(result.ApartmentOrders.Select(x => x.Order)) : new List<OrderModel>();

            outputModel.Orders = outputOrderModels;

            return new ResponseWrapper<ApartmentModel>(outputModel);
        }

        public async Task<ResponseWrapper<ApartmentModel>> InsertAsync(ApartmentModel model)
        {
            var result = await _dBRepository.ApartmentRepo.InsertAsync(_mapper.Map<Apartment>(model));

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<ApartmentModel>(_mapper.Map<ApartmentModel>(result));
        }

        public async Task<ResponseWrapper<ApartmentModel>> DeleteApartmentFromAllOrdersAsync(string apartmentId)
        {
            var result = await _dBRepository.ApartmentRepo.DeleteApartmentFromAllOrdersAsync(apartmentId);

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<ApartmentModel>(_mapper.Map<ApartmentModel>(result));
        }

        public async Task<ResponseWrapper<ApartmentModel>> UpdateAsync(string id, ApartmentModel model)
        {
            var result = await _dBRepository.ApartmentRepo.UpdateAsync(id, _mapper.Map<Apartment>(model));

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<ApartmentModel>(_mapper.Map<ApartmentModel>(result));
        }
    }
}
