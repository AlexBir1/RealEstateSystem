using DwellingAPI.DAL.DBContext;
using DwellingAPI.DAL.Entities;
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

        public async Task<ResponseWrapper<Order>> DeleteAsync(string id)
        {
            try
            {
                var order = await _db.Orders.SingleOrDefaultAsync(x=>x.Id == Guid.Parse(id));

                if(order == null)
                    return new ResponseWrapper<Order>(new List<string> { new string("Order does not exist.") });

                _db.Orders.Remove(order);

                return new ResponseWrapper<Order>(order);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Order>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<Order>>> GetAllAsync()
        {
            try
            {
                var orders = await _db.Orders.ToListAsync();

                if (orders.IsNullOrEmpty())
                    return new ResponseWrapper<IEnumerable<Order>>(new List<string> { new string("List of orders is empty") });

                return new ResponseWrapper<IEnumerable<Order>>(orders);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<IEnumerable<Order>>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<Order>>> GetAllByAccountIdAsync(string accountId)
        {
            try
            {
                var orders = await _db.Orders.Where(x => x.AccountId == accountId).ToListAsync();

                if (orders.IsNullOrEmpty())
                    return new ResponseWrapper<IEnumerable<Order>>(new List<string> { new string("List of orders is empty") });

                return new ResponseWrapper<IEnumerable<Order>>(orders);
            }
            catch(Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<IEnumerable<Order>>(errors);
            }
        }

        public async Task<ResponseWrapper<Order>> GetByIdAsync(string id)
        {
            try
            {
                var order = await _db.Orders
                    .Include(x=>x.OrderApartments).ThenInclude(x=>x.Apartment)
                    .SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (order == null)
                    return new ResponseWrapper<Order>(new List<string> { new string("Order does not exist.") });

                return new ResponseWrapper<Order>(order);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Order>(errors);
            }
        }

        public async Task<ResponseWrapper<Order>> InsertAsync(Order entity)
        {
            try
            {
                entity.CreationDate = DateTime.Now;
                entity.LastlyUpdatedDate = DateTime.Now;

                var result = await _db.Orders.AddAsync(entity);

                return new ResponseWrapper<Order>(result.Entity);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Order>(errors);
            }
        }

        public async Task<ResponseWrapper<Order>> RemoveApartmentsAsync(string orderId, IEnumerable<string> apartmentIds)
        {
            try
            {
                var order = await _db.Orders.Include(x=>x.OrderApartments).AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(orderId));

                if(order == null)
                    return new ResponseWrapper<Order>(new List<string> { new string("Order does not exist.") });

                _db.OrdersApartments.RemoveRange(apartmentIds.Select(x => new OrderApartment { ApartmentId = Guid.Parse(x), OrderId = Guid.Parse(orderId) }));

                foreach(var apartmentId in apartmentIds)
                {
                    order.OrderApartments.Remove(order.OrderApartments.Single(x=>x.OrderId == Guid.Parse(orderId) && x.ApartmentId == Guid.Parse(apartmentId)));
                }
                    

                return new ResponseWrapper<Order>(order);

            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException!.Message) : string.Empty,
                };

                return new ResponseWrapper<Order>(errors);
            }
        }

        public async Task<ResponseWrapper<Order>> UpdateAsync(string id, Order entity)
        {
            try
            {
                var order = await _db.Orders.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (order == null)
                    return new ResponseWrapper<Order>(new List<string> { new string("Order does not exist.") });

                entity.CreationDate = order.CreationDate;
                entity.EstimatedRoomsQuantity = order.EstimatedRoomsQuantity;
                entity.City = order.City;
                entity.EstimatedPriceLimit = order.EstimatedPriceLimit;
                entity.AccountId = order.AccountId;
                entity.LastlyUpdatedDate = DateTime.Now;

                _db.Orders.Update(entity);

                return new ResponseWrapper<Order>(order);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException!.Message) : string.Empty,
                };

                return new ResponseWrapper<Order>(errors);
            }
        }
    }
}
