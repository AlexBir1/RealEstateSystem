using Bogus;
using Bogus.DataSets;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Repositories;
using DwellingAPI.xUnit.DbContext;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.xUnit.Repositories
{
    public class AgreementRepositoryTest
    {
        private readonly Faker<Agreement> AgreementFaker;

        public AgreementRepositoryTest() 
        {
            AgreementFaker = new Faker<Agreement>()
                .RuleFor(x => x.Id, () => Guid.NewGuid())
                .RuleFor(x => x.AccountId, f => f.Lorem.Word())
                .RuleFor(x => x.ApartmentCity, f => f.Lorem.Word())
                .RuleFor(x => x.ApartmentAddress, f => f.Lorem.Sentence(5))
                .RuleFor(x => x.IsActive, () => true)
                .RuleFor(x => x.SumPerMonth, new Random().Next(1000,9999));
        }

        [Fact]
        public async void GetAllAsync_IfCountGreaterThanZero()
        {
            // Arrange
            var agreements = AgreementFaker.Generate(5);
            var db = TestDBContext.GetTestDBContext();

            await db.Agreements.AddRangeAsync(agreements);
            await db.SaveChangesAsync();

            // Act
            var repo = new AgreementRepository(db);
            var result = await repo.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(agreements.Count, result.Count());

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_IfCountIsZero()
        {
            // Arrange
            var db = TestDBContext.GetTestDBContext();

            // Act
            var repo = new AgreementRepository(db);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.GetAllAsync());

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetByIdAsync_IfExists()
        {
            // Arrange
            var agreement = AgreementFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            await db.Agreements.AddAsync(agreement);
            await db.SaveChangesAsync();

            // Act
            var repo = new AgreementRepository(db);
            var result = await repo.GetByIdAsync(agreement.Id.ToString());

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void UpdateAsync_IfExists()
        {
            // Arrange
            var agreement = AgreementFaker.Generate();
            var agreement2 = AgreementFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            agreement2.Id = agreement.Id;

            await db.Agreements.AddAsync(agreement);
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();

            // Act
            var repo = new AgreementRepository(db);
            var result = await repo.UpdateAsync(agreement2.Id.ToString(), agreement2);
            await db.SaveChangesAsync();

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void UpdateAsync_IfNotExists()
        {
            // Arrange
            var agreement = AgreementFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            // Act
            var repo = new AgreementRepository(db);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.UpdateAsync(agreement.Id.ToString(), agreement));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_IfExists()
        {
            // Arrange
            var agreement = AgreementFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            await db.Agreements.AddAsync(agreement);
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();

            // Act
            var repo = new AgreementRepository(db);
            var result = await repo.DeleteAsync(agreement.Id.ToString());
            await db.SaveChangesAsync();

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_IfNotExists()
        {
            // Arrange
            var agreement = AgreementFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            // Act
            var repo = new AgreementRepository(db);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.DeleteAsync(agreement.Id.ToString()));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetByIdAsync_IfNotExists()
        {
            // Arrange
            var agreement = AgreementFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            // Act
            var repo = new AgreementRepository(db);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.GetByIdAsync(agreement.Id.ToString()));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllByAccountIdAsync_IfCountGreaterThanZero()
        {
            // Arrange
            var agreements = AgreementFaker.Generate(5);
            var db = TestDBContext.GetTestDBContext();

            string accountId = Guid.NewGuid().ToString();

            agreements.ForEach(x => x.AccountId = accountId);

            await db.Agreements.AddRangeAsync(agreements);
            await db.SaveChangesAsync();

            // Act
            var repo = new AgreementRepository(db);
            var result = await repo.GetAllByAccountIdAsync(accountId);

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllByAccountIdAsync_IfCountIsZero()
        {
            // Arrange
            var db = TestDBContext.GetTestDBContext();
            string accountId = Guid.NewGuid().ToString();

            // Act
            var repo = new AgreementRepository(db);

            //Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.GetAllByAccountIdAsync(accountId));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }
    }
}
