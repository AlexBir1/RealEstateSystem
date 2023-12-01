using Bogus;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Repositories;
using DwellingAPI.xUnit.DbContext;
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

        [Fact]
        public async void InsertAsync_SeveralValues()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var orders = OrderFaker.Generate(10);

            //Act
            var repo = new OrderRepository(db);

            foreach (var item in orders) 
                await repo.InsertAsync(item);
            await db.SaveChangesAsync();

            var result = await repo.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);
            Assert.Equal(result.Data.Count(), db.Orders.Count());

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void InsertAsync_OneValue()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var order = OrderFaker.Generate();

            //Act
            var repo = new OrderRepository(db);
            var result = await repo.InsertAsync(order);
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]  
        public async void UpdateAsync_IfExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var order = OrderFaker.Generate();
            var newOrder = OrderFaker.Generate();

            newOrder.Id = order.Id;

            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();

            //Act
            var repo = new OrderRepository(db);
            var result = await repo.UpdateAsync(newOrder.Id.ToString(), newOrder);
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void UpdateAsync_IfNotExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var order = OrderFaker.Generate();

            //Act
            var repo = new OrderRepository(db);
            var result = await repo.UpdateAsync(order.Id.ToString(), order);
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_IfExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var order = OrderFaker.Generate();

            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();

            //Act
            var repo = new OrderRepository(db);
            var result = await repo.DeleteAsync(order.Id.ToString());
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_IfNotExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var order = OrderFaker.Generate();

            //Act
            var repo = new OrderRepository(db);
            var result = await repo.DeleteAsync(order.Id.ToString());
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_IfCountGreaterThanZero()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var order = OrderFaker.Generate(3);

            await db.Orders.AddRangeAsync(order);
            await db.SaveChangesAsync();

            //Act
            var repo = new OrderRepository(db);
            var result = await repo.GetAllAsync();
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_IfCountIsZero()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var order = OrderFaker.Generate();

            //Act
            var repo = new OrderRepository(db);
            var result = await repo.GetAllAsync();
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllByAccountIdAsync_IfCountGreaterThanZero()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            Guid accountId = Guid.NewGuid();
            var order = OrderFaker.RuleFor(x => x.AccountId, () => accountId.ToString()).Generate(3);

            await db.Orders.AddRangeAsync(order);
            await db.SaveChangesAsync();

            //Act
            var repo = new OrderRepository(db);
            var result = await repo.GetAllAsync();
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllByAccountIdAsync_IfCountIsZero()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            //Act
            var repo = new OrderRepository(db);
            var result = await repo.GetAllAsync();
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetByIdAsync_IfExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var order = OrderFaker.Generate();

            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();

            //Act
            var repo = new OrderRepository(db);
            var result = await repo.GetByIdAsync(order.Id.ToString());
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetByIdAsync_IfNotExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var order = OrderFaker.Generate();

            //Act
            var repo = new OrderRepository(db);
            var result = await repo.GetByIdAsync(order.Id.ToString());
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }
    }
}
