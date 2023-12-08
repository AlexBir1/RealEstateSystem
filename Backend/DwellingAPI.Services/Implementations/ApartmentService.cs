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
            try
            {
                var result = await _dBRepository.ApartmentRepo.AddMainPhotoAsync(model.ApartmentId.ToString(), model.PhotoFile);
                if (result.Data == null)
                    return new ResponseWrapper<ApartmentModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<ApartmentModel>(commitResult.Errors);

                var outputModel = _mapper.Map<ApartmentModel>(result.Data);
                return new ResponseWrapper<ApartmentModel>(outputModel);
            }
            catch(Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<ApartmentModel>(errors);
            }
        }

        public async Task<ResponseWrapper<ApartmentPhotoModel>> AddPhotoAsync(ApartmentPhotoModel model)
        {
            try
            {
                var result = await _dBRepository.ApartmentRepo.AddPhotoAsync(_mapper.Map<ApartmentPhoto>(model), model.PhotoFile);
                if (result.Data == null)
                    return new ResponseWrapper<ApartmentPhotoModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<ApartmentPhotoModel>(commitResult.Errors);

                var outputModel = _mapper.Map<ApartmentPhotoModel>(result.Data);
                return new ResponseWrapper<ApartmentPhotoModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<ApartmentPhotoModel>(errors);
            }
        }

        public async Task<ResponseWrapper<ApartmentModel>> DeleteAsync(string id)
        {
            try
            {
                var result = await _dBRepository.ApartmentRepo.DeleteAsync(id);
                if (result.Data == null)
                    return new ResponseWrapper<ApartmentModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<ApartmentModel>(commitResult.Errors);

                var outputModel = _mapper.Map<ApartmentModel>(result.Data);
                return new ResponseWrapper<ApartmentModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<ApartmentModel>(errors);
            }
        }

        public async Task<ResponseWrapper<ApartmentModel>> DeleteMainPhotoAsync(string apartmentId)
        {
            try
            {
                var result = await _dBRepository.ApartmentRepo.DeleteMainPhotoAsync(apartmentId);
                if (result.Data == null)
                    return new ResponseWrapper<ApartmentModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<ApartmentModel>(commitResult.Errors);

                var outputModel = _mapper.Map<ApartmentModel>(result.Data);
                return new ResponseWrapper<ApartmentModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<ApartmentModel>(errors);
            }
        }

        public async Task<ResponseWrapper<ApartmentModel>> DeletePhotoAsync(string apartmentId, string photoId)
        {
            try
            {
                var result = await _dBRepository.ApartmentRepo.DeletePhotoAsync(apartmentId, photoId);
                if (result.Data == null)
                    return new ResponseWrapper<ApartmentModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<ApartmentModel>(commitResult.Errors);

                var outputModel = _mapper.Map<ApartmentModel>(result.Data);
                return new ResponseWrapper<ApartmentModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<ApartmentModel>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<ApartmentModel>>> GetAllAsync()
        {
            try
            {
                var result = await _dBRepository.ApartmentRepo.GetAllAsync();
                if (result.Data == null)
                    return new ResponseWrapper<IEnumerable<ApartmentModel>>(result.Errors);

                var outputModel = _mapper.Map<IEnumerable<ApartmentModel>>(result.Data);
                return new ResponseWrapper<IEnumerable<ApartmentModel>>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<IEnumerable<ApartmentModel>>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<ApartmentModel>>> GetAllByOrderRequirementsAsync(string orderId)
        {
            try
            {
                var result = await _dBRepository.ApartmentRepo.GetAllByOrderRequirementsAsync(orderId);
                if (result.Data == null)
                    return new ResponseWrapper<IEnumerable<ApartmentModel>>(result.Errors);

                var outputModel = _mapper.Map<IEnumerable<ApartmentModel>>(result.Data);
                return new ResponseWrapper<IEnumerable<ApartmentModel>>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<IEnumerable<ApartmentModel>>(errors);
            }
        }

        public async Task<ResponseWrapper<ApartmentModel>> GetAllPhotosAsync(string apartmentId)
        {
            try
            {
                var result = await _dBRepository.ApartmentRepo.GetAllPhotosAsync(apartmentId);
                if (result.Data == null)
                    return new ResponseWrapper<ApartmentModel>(result.Errors);

                var outputModel = _mapper.Map<ApartmentModel>(result.Data);
                return new ResponseWrapper<ApartmentModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<ApartmentModel>(errors);
            }
        }

        public async Task<ResponseWrapper<ApartmentModel>> GetByIdAsync(string id)
        {
            try
            {
                var result = await _dBRepository.ApartmentRepo.GetByIdAsync(id);
                if (result.Data == null)
                    return new ResponseWrapper<ApartmentModel>(result.Errors);

                var outputModel = _mapper.Map<ApartmentModel>(result.Data);

                var outputOrderModels = !result.Data.ApartmentOrders.IsNullOrEmpty() ? _mapper.Map<ICollection<OrderModel>>(result.Data.ApartmentOrders.Select(x=>x.Order)) : new List<OrderModel>();

                outputModel.Orders = outputOrderModels;

                return new ResponseWrapper<ApartmentModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<ApartmentModel>(errors);
            }
        }

        public async Task<ResponseWrapper<ApartmentModel>> InsertAsync(ApartmentModel model)
        {
            try
            {
                var result = await _dBRepository.ApartmentRepo.InsertAsync(_mapper.Map<Apartment>(model));
                if (result.Data == null)
                    return new ResponseWrapper<ApartmentModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<ApartmentModel>(commitResult.Errors);

                var outputModel = _mapper.Map<ApartmentModel>(result.Data);
                return new ResponseWrapper<ApartmentModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<ApartmentModel>(errors);
            }
        }

        public async Task<ResponseWrapper<ApartmentModel>> DeleteApartmentFromAllOrdersAsync(string apartmentId)
        {
            try
            {
                var result = await _dBRepository.ApartmentRepo.DeleteApartmentFromAllOrdersAsync(apartmentId);

                if (result.Data == null)
                    return new ResponseWrapper<ApartmentModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<ApartmentModel>(commitResult.Errors);

                var outputModel = _mapper.Map<ApartmentModel>(result.Data);

                return new ResponseWrapper<ApartmentModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<ApartmentModel>(errors);
            }
        }

        public async Task<ResponseWrapper<ApartmentModel>> UpdateAsync(string id, ApartmentModel model)
        {
            try
            {
                var result = await _dBRepository.ApartmentRepo.UpdateAsync(id, _mapper.Map<Apartment>(model));
                if (result.Data == null)
                    return new ResponseWrapper<ApartmentModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<ApartmentModel>(commitResult.Errors);

                var outputModel = _mapper.Map<ApartmentModel>(result.Data);
                return new ResponseWrapper<ApartmentModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<ApartmentModel>(errors);
            }
        }
    }
}
