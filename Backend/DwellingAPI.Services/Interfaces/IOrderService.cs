using DwellingAPI.DAL.Entities;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseWrapper<OrderModel>> InsertAsync(OrderModel model);
        Task<ResponseWrapper<OrderModel>> UpdateAsync(string id, OrderModel model);
        Task<ResponseWrapper<OrderModel>> UpdateApartmentsAsync(string id, IEnumerable<ApartmentModel> models);
        Task<ResponseWrapper<OrderModel>> RemoveApartmentsAsync(string id, IEnumerable<ApartmentModel> models);
        Task<ResponseWrapper<OrderModel>> DeleteAsync(string id);
        Task<ResponseWrapper<IEnumerable<OrderModel>>> GetAllAsync();
        Task<ResponseWrapper<OrderModel>> GetByIdAsync(string id);
        Task<ResponseWrapper<IEnumerable<OrderModel>>> GetAllByAccountIdAsync(string accountId);
    }
}
