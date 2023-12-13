using Bogus;
using DwellingAPI.DAL.DBContext;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Repositories;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.xUnit.DbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.xUnit.Repositories
{
    public class ApartmentsRepositoryTest
    {
        private readonly Faker<Apartment> ApartmentFaker;
        private readonly Faker<Order> OrderFaker;

        public ApartmentsRepositoryTest()
        {
            ApartmentFaker = new Faker<Apartment>()
                .RuleFor(x => x.Id, () => Guid.NewGuid())
                .RuleFor(x => x.Number, f => f.Lorem.Word())
                .RuleFor(x => x.Price, new Random().Next(100000))
                .RuleFor(x => x.ImageUrl, f => f.Lorem.Word());

            OrderFaker = new Faker<Order>()
                .RuleFor(x => x.Id, () => Guid.NewGuid())
                .RuleFor(x => x.City, () => Guid.NewGuid().ToString())
                .RuleFor(x=>x.AccountId, f => f.Lorem.Word());
        }

        [Fact]
        public async void GetAllAsync_IfCountGreaterThanZero()
        {
            // Arrange
            var apartments = ApartmentFaker.Generate(5);
            var db = TestDBContext.GetTestDBContext();

            await db.Apartments.AddRangeAsync(apartments);
            await db.SaveChangesAsync();

            // Act
            var repo = new ApartmentRepository(db);
            var result = await repo.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_IfCountZero()
        {
            // Arrange
            var db = TestDBContext.GetTestDBContext();

            // Act
            var repo = new ApartmentRepository(db);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.GetAllAsync());

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetByIdAsync_IfItemExists()
        {
            // Arrange
            var detailsFaker = new Faker<ApartmentDetails>()
                .RuleFor(x => x.ApartmentId, () => Guid.NewGuid())
                .RuleFor(x => x.RealtorName, f => f.Lorem.Word())
                .RuleFor(x => x.RealtorPhone, f => f.Lorem.Word())
                .RuleFor(x => x.Description, f => f.Lorem.Sentence(10));
            ApartmentFaker.RuleFor(x => x.Details, detailsFaker.Generate());

            var apartment = ApartmentFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            await db.Apartments.AddAsync(apartment);
            await db.SaveChangesAsync();

            // Act
            var repo = new ApartmentRepository(db);
            var result = await repo.GetByIdAsync(apartment.Id.ToString());

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetByIdAsync_IfItemNotExists()
        {
            // Arrange
            var apartment = ApartmentFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            // Act
            var repo = new ApartmentRepository(db);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.GetByIdAsync(apartment.Id.ToString()));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllPhotosAsync_IfCountGreaterThanZero()
        {
            // Arrange
            var photoFaker = new Faker<ApartmentPhoto>()
                .RuleFor(x => x.Id, () => Guid.NewGuid())
                .RuleFor(x => x.ImageUrl, f => f.Lorem.Word());
            ApartmentFaker.RuleFor(x => x.Photos, photoFaker.Generate(3));

            var apartment = ApartmentFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            await db.Apartments.AddAsync(apartment);
            await db.SaveChangesAsync();

            // Act
            var repo = new ApartmentRepository(db);
            var result = await repo.GetAllPhotosAsync(apartment.Id.ToString());

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_IfItemExists()
        {
            // Arrange
            var apartment = ApartmentFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            await db.Apartments.AddAsync(apartment);
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();

            // Act
            var repo = new ApartmentRepository(db);
            var result = await repo.DeleteAsync(apartment.Id.ToString());

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_IfItemNotExists()
        {
            // Arrange
            var apartment = ApartmentFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            // Act
            var repo = new ApartmentRepository(db);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.DeleteAsync(apartment.Id.ToString()));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void InsertAsync_IfItemNotExists()
        {
            // Arrange
            var apartment = ApartmentFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            // Act
            var repo = new ApartmentRepository(db);
            var result = await repo.InsertAsync(apartment);

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void InsertAsync_IfItemExists()
        {
            // Arrange
            var apartment = ApartmentFaker.Generate();
            var db = TestDBContext.GetTestDBContext();
            await db.Apartments.AddAsync(apartment);
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();

            // Act
            var repo = new ApartmentRepository(db);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.InsertAsync(apartment));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void UpdateAsync_IfItemExists()
        {
            // Arrange
            var apartment = ApartmentFaker.Generate();
            var apartment2 = ApartmentFaker.Generate();

            apartment2.Id = apartment.Id;

            var db = TestDBContext.GetTestDBContext();
            await db.Apartments.AddAsync(apartment);
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();

            // Act
            var repo = new ApartmentRepository(db);
            var result = await repo.UpdateAsync(apartment.Id.ToString(), apartment2);

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void UpdateAsync_IfItemNotExists()
        {
            // Arrange
            var apartment = ApartmentFaker.Generate();

            var db = TestDBContext.GetTestDBContext();

            // Act
            var repo = new ApartmentRepository(db);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.UpdateAsync(apartment.Id.ToString(), apartment));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        // coming soon
        public async void AddPhotoAsync_IfItemNotExists()
        {
            // Arrange
            var apartment = ApartmentFaker.Generate();

            var db = TestDBContext.GetTestDBContext();
            await db.Apartments.AddAsync(apartment);
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();


            // Act
            var repo = new ApartmentRepository(db);

            // Assert

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        // coming soon
        public async void AddMainPhotoAsync_IfItemNotExists()
        {
            // Arrange
            var apartment = ApartmentFaker.Generate();

            var db = TestDBContext.GetTestDBContext();
            await db.Apartments.AddAsync(apartment);
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();

            // Act
            var repo = new ApartmentRepository(db);

            // Assert

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllByAccountIdAsync_IfCountGreaterThanZero()
        {
            // Arrange
            var apartments = ApartmentFaker.Generate(5);
            var order = OrderFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            List<OrderApartment> orderApartments = new List<OrderApartment>();

            foreach(var apartment in apartments)
            {
                var orderApartment = new OrderApartment()
                {
                    Order = order,
                    Apartment = apartment
                };

                orderApartments.Add(orderApartment);
            }

            await db.Apartments.AddRangeAsync(apartments);
            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();

            await db.OrdersApartments.AddRangeAsync(orderApartments);
            await db.SaveChangesAsync();

            // Act
            var repo = new ApartmentRepository(db);
            var result = await repo.GetAllByAccountIdAsync(order.AccountId);

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }
    }
}
