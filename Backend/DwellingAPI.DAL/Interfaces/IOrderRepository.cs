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
        Task<IEnumerable<Order>> GetAllByAccountIdAsync(string accountId);
        Task<Order> RemoveApartmentsAsync(string orderId, IEnumerable<string> apartmentIds);
        

        Task<Order> ChangeStatusAsync(string orderId, OrderStatus status);
    }
}
