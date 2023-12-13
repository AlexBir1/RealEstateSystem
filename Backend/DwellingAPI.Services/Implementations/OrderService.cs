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
            var result = await _dBRepository.OrderRepo.DeleteAsync(id);

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<OrderModel>(_mapper.Map<OrderModel>(result));
        }

        public async Task<ResponseWrapper<IEnumerable<OrderModel>>> GetAllAsync(string accountId)
        {
            IEnumerable<Order> result;

            if (string.IsNullOrEmpty(accountId))
                result = await _dBRepository.OrderRepo.GetAllAsync();
            else
                result = await _dBRepository.OrderRepo.GetAllByAccountIdAsync(accountId);

            return new ResponseWrapper<IEnumerable<OrderModel>>(_mapper.Map<IEnumerable<OrderModel>>(result));
        }

        public async Task<ResponseWrapper<OrderModel>> GetByIdAsync(string id)
        {
            var result = await _dBRepository.OrderRepo.GetByIdAsync(id);

            var outputModel = _mapper.Map<OrderModel>(result);

            var outputApartmentModels = _mapper.Map<ICollection<ApartmentModel>>(result.OrderApartments.Select(x => x.Apartment));

            outputModel.Apartments = outputApartmentModels;

            return new ResponseWrapper<OrderModel>(outputModel);
        }

        public async Task<ResponseWrapper<OrderModel>> InsertAsync(OrderModel model)
        {
            var result = await _dBRepository.OrderRepo.InsertAsync(_mapper.Map<Order>(model));

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<OrderModel>(_mapper.Map<OrderModel>(result));
        }

        public async Task<ResponseWrapper<OrderModel>> UpdateApartmentsAsync(string id, IEnumerable<ApartmentModel> models)
        {
            var order = new Order
            {
                Id = Guid.Parse(id),
                OrderStatus = OrderStatus.FoundApartment,
                OrderApartments = models.Select(x => new OrderApartment { ApartmentId = Guid.Parse(x.Id) }).ToList()
            };

            var result = await _dBRepository.OrderRepo.UpdateAsync(id, order);

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<OrderModel>(_mapper.Map<OrderModel>(result));
        }

        public async Task<ResponseWrapper<OrderModel>> RemoveApartmentsAsync(string id, IEnumerable<ApartmentModel> models)
        {
            var result = await _dBRepository.OrderRepo.RemoveApartmentsAsync(id, models.Select(x => x.Id));

            if (result.OrderApartments.IsNullOrEmpty())
                await _dBRepository.OrderRepo.ChangeStatusAsync(id, OrderStatus.SearchForApartment);

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<OrderModel>(_mapper.Map<OrderModel>(result));
        }

        public async Task<ResponseWrapper<OrderModel>> UpdateAsync(string id, OrderModel model)
        {
            var result = await _dBRepository.OrderRepo.UpdateAsync(id, _mapper.Map<Order>(model));

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<OrderModel>(_mapper.Map<OrderModel>(result));
        }

        public async Task<ResponseWrapper<OrderModel>> ChangeStatusAsync(string id, OrderStatus status)
        {
            var result = await _dBRepository.OrderRepo.ChangeStatusAsync(id, status);

            await _dBRepository.CommitAsync();

            return new ResponseWrapper<OrderModel>(_mapper.Map<OrderModel>(result));
        }
    }
}
