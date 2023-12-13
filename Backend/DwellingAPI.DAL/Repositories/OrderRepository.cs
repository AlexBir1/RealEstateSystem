using DwellingAPI.DAL.DBContext;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Exceptions;
using DwellingAPI.DAL.Interfaces;
using DwellingAPI.ResponseWrapper.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDBContext _db;

        public OrderRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<Order> ChangeStatusAsync(string orderId, OrderStatus status)
        {
            var order = await _db.Orders.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(orderId));

            if (order == null)
                throw new OperationFailedException("Order is not found");

            order.OrderStatus = status;

            return order;
        }

        public async Task<Order> DeleteAsync(string id)
        {
            var order = await _db.Orders.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (order == null)
                throw new OperationFailedException("Order is not found");

            return _db.Orders.Remove(order).Entity;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var orders = await _db.Orders.AsNoTracking().ToListAsync();

            if (orders.IsNullOrEmpty())
                throw new OperationFailedException("No order were found");

            return orders;
        }

        public async Task<IEnumerable<Order>> GetAllByAccountIdAsync(string accountId)
        {
            var orders = await _db.Orders.Where(x => x.AccountId == accountId).AsNoTracking().ToListAsync();

            if (orders.IsNullOrEmpty())
                throw new OperationFailedException("No order were found");

            return orders;
        }

        public async Task<Order> GetByIdAsync(string id)
        {
            var order = await _db.Orders
                .Include(x => x.OrderApartments).ThenInclude(x => x.Apartment)
                .AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (order == null)
                throw new OperationFailedException("Order is not found");

            return order;
        }

        public async Task<Order> InsertAsync(Order entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.LastlyUpdatedDate = DateTime.Now;
            entity.OrderStatus = OrderStatus.InProcess;

            var result = await _db.Orders.AddAsync(entity);

            return result.Entity;
        }

        public async Task<Order> RemoveApartmentsAsync(string orderId, IEnumerable<string> apartmentIds)
        {
            var order = await _db.Orders.Include(x => x.OrderApartments).AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(orderId));

            if (order == null)
                throw new OperationFailedException("Order is not found");

            _db.OrdersApartments.RemoveRange(apartmentIds.Select(x => new OrderApartment { ApartmentId = Guid.Parse(x), OrderId = Guid.Parse(orderId) }));

            foreach (var apartmentId in apartmentIds)
            {
                order.OrderApartments.Remove(order.OrderApartments.Single(x => x.OrderId == Guid.Parse(orderId) && x.ApartmentId == Guid.Parse(apartmentId)));
            }

            return order;
        }

        public async Task<Order> UpdateAsync(string id, Order entity)
        {
            var order = await _db.Orders.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (order == null)
                throw new OperationFailedException("Order is not found");

            entity.CreationDate = order.CreationDate;
            entity.EstimatedRoomsQuantity = order.EstimatedRoomsQuantity;
            entity.City = order.City;
            entity.EstimatedPriceLimit = order.EstimatedPriceLimit;
            entity.AccountId = order.AccountId;
            entity.LastlyUpdatedDate = DateTime.Now;

            return _db.Orders.Update(entity).Entity;
        }
    }
}
