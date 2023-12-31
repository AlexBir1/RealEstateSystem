﻿using Bogus;
using DwellingAPI.DAL.DBContext;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Repositories;
using DwellingAPI.xUnit.DbContext;
using DwellingAPI.xUnit.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DwellingAPI.xUnit.Repositories
{
    public class CallRepositoryTest
    {
        private readonly Faker<Call> CallFaker;

        public CallRepositoryTest()
        {
            CallFaker = new Faker<Call>()
                .RuleFor(x => x.Id, () => Guid.NewGuid())
                .RuleFor(x => x.IsCompleted, Convert.ToBoolean(new Random().Next(0,1)))
                .RuleFor(x => x.ToPhone, f => f.Lorem.Sentence(3))
                .RuleFor(x => x.ToName, f => f.Lorem.Sentence(3))
                .RuleFor(x => x.CreationDate, DateTime.Now);
        }

        [Fact]
        public async void InsertAsync_JustAddNewValue()
        {
            // Arrange
            var call = CallFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            // Act
            var repo = new CallRepository(db);
            var result = await repo.InsertAsync(call);
            await db.SaveChangesAsync();

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_IfCountGreaterThanZero()
        {
            // Arrange
            var calls = CallFaker.Generate(3);
            var db = TestDBContext.GetTestDBContext();

            await db.Calls.AddRangeAsync(calls);
            await db.SaveChangesAsync();

            // Act
            var repo = new CallRepository(db);
            var result = await repo.GetAllAsync();

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_IfCountIsZero()
        {
            // Arrange
            var db = TestDBContext.GetTestDBContext();

            // Act
            var repo = new CallRepository(db);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.GetAllAsync());

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void UpdateAsync_IfValueNotExists()
        {
            // Arrange
            var call2 = CallFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            // Act
            var repo = new CallRepository(db);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.UpdateAsync(call2.Id.ToString(), call2));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void UpdateAsync_IfValueExists()
        {
            // Arrange
            var call = CallFaker.Generate();
            var call2 = CallFaker.Generate();

            call2.Id = call.Id;

            var db = TestDBContext.GetTestDBContext();

            await db.Calls.AddAsync(call);
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();

            // Act
            var repo = new CallRepository(db);
            var result = await repo.UpdateAsync(call.Id.ToString(), call2); ;
            await db.SaveChangesAsync();

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_IfValueNotExists()
        {
            // Arrange
            var call = CallFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            // Act
            var repo = new CallRepository(db);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.DeleteAsync(call.Id.ToString()));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_IfValueExists()
        {
            // Arrange
            var call = CallFaker.Generate();
            var db = TestDBContext.GetTestDBContext();

            await db.Calls.AddAsync(call);
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();

            // Act
            var repo = new CallRepository(db);
            var result = await repo.GetByIdAsync(call.Id.ToString());
            await db.SaveChangesAsync();

            // Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }
    }
}
