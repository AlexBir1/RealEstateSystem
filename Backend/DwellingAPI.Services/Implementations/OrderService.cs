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
    public class OrderService : IOrderService
    {
        private readonly IDBRepository _dBRepository;
        private readonly IMapper _mapper;

        public OrderService(IDBRepository dBRepository, IMapper mapper)
        {
            _dBRepository = dBRepository;
            _mapper = mapper;
        }

        public async Task<ResponseWrapper<OrderModel>> DeleteAsync(string id)
        {
            try
            {
                var result = await _dBRepository.OrderRepo.DeleteAsync(id);

                if(result == null)
                    return new ResponseWrapper<OrderModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<OrderModel>(commitResult.Errors);

                return new ResponseWrapper<OrderModel>(_mapper.Map<OrderModel>(result.Data));
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<OrderModel>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<OrderModel>>> GetAllAsync()
        {
            try
            {
                var result = await _dBRepository.OrderRepo.GetAllAsync();

                if (result == null)
                    return new ResponseWrapper<IEnumerable<OrderModel>>(result.Errors);

                return new ResponseWrapper<IEnumerable<OrderModel>>(_mapper.Map<IEnumerable<OrderModel>>(result.Data));
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<IEnumerable<OrderModel>>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<OrderModel>>> GetAllByAccountIdAsync(string accountId)
        {
            try
            {
                var result = await _dBRepository.OrderRepo.GetAllByAccountIdAsync(accountId);

                if (result == null)
                    return new ResponseWrapper<IEnumerable<OrderModel>>(result.Errors);

                return new ResponseWrapper<IEnumerable<OrderModel>>(_mapper.Map<IEnumerable<OrderModel>>(result.Data));
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<IEnumerable<OrderModel>>(errors);
            }
        }

        public async Task<ResponseWrapper<OrderModel>> GetByIdAsync(string id)
        {
            try
            {
                var result = await _dBRepository.OrderRepo.GetByIdAsync(id);

                if (result == null)
                    return new ResponseWrapper<OrderModel>(result.Errors);

                var outputModel = _mapper.Map<OrderModel>(result.Data);

                var outputApartmentModels = _mapper.Map<ICollection<ApartmentModel>>(result.Data.OrderApartments.Select(x => x.Apartment));

                outputModel.Apartments = outputApartmentModels;

                return new ResponseWrapper<OrderModel>(outputModel);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<OrderModel>(errors);
            }
        }

        public async Task<ResponseWrapper<OrderModel>> InsertAsync(OrderModel model)
        {
            try
            {
                var result = await _dBRepository.OrderRepo.InsertAsync(_mapper.Map<Order>(model));

                if (result == null)
                    return new ResponseWrapper<OrderModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<OrderModel>(commitResult.Errors);

                return new ResponseWrapper<OrderModel>(_mapper.Map<OrderModel>(result.Data));
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<OrderModel>(errors);
            }
        }

        public async Task<ResponseWrapper<OrderModel>> UpdateAsync(string id, OrderModel model)
        {
            try
            {
                var result = await _dBRepository.OrderRepo.UpdateAsync(id, _mapper.Map<Order>(model));

                if (result == null)
                    return new ResponseWrapper<OrderModel>(result.Errors);

                var commitResult = await _dBRepository.CommitAsync();

                if (commitResult.Errors.Any())
                    return new ResponseWrapper<OrderModel>(commitResult.Errors);

                return new ResponseWrapper<OrderModel>(_mapper.Map<OrderModel>(result.Data));
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<OrderModel>(errors);
            }
        }
    }
}
