using Bogus;
using DwellingAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.xUnit.Repositories
{
    public class OrderRepositoryTest
    {
        private readonly Faker<Order> OrderFaker;

        public OrderRepositoryTest()
        {
            OrderFaker = new Faker<Order>()
                .RuleFor(x => x.Id, () => Guid.NewGuid())
                .RuleFor(x => x.OrderStatus, f => OrderStatus.InProcess)
                .RuleFor(x => x.AccountId, f => Guid.NewGuid().ToString())
                .RuleFor(x => x.CreationDate, DateTime.Now)
                .RuleFor(x => x.LastlyUpdatedDate, DateTime.Now);
        }
    }
}
