using DwellingAPI.DAL.Entities;
using DwellingAPI.ResponseWrapper.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<ResponseWrapper<IEnumerable<Order>>> GetAllByAccountIdAsync(string accountId);
        Task<ResponseWrapper<Order>> RemoveApartmentsAsync(string orderId, IEnumerable<string> apartmentIds);
        

        Task<ResponseWrapper<Order>> ChangeStatusAsync(string orderId, OrderStatus status);
    }
}
