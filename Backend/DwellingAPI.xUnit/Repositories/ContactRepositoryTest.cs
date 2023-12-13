using Bogus;
using Bogus.DataSets;
using DwellingAPI.DAL.DBContext;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Repositories;
using DwellingAPI.xUnit.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.xUnit.Repositories
{
    public class ContactRepositoryTest
    {
        private readonly Faker<Contact> ConatactFaker;

        public ContactRepositoryTest()
        {
            ConatactFaker = new Faker<Contact>()
                .RuleFor(x => x.Id, () => Guid.NewGuid())
                .RuleFor(x => x.ContactOptionName, f => f.Lorem.Sentence(1))
                .RuleFor(x => x.ContactOptionValue, f=>f.Phone.ToString())
                .RuleFor(x => x.CreationDate, DateTime.Now)
                .RuleFor(x => x.LastlyUpdatedDate, DateTime.Now);
        }

        [Fact]
        public async void InsertAsync_IfNotExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var contact = ConatactFaker.Generate();

            //Act
            var repo = new ContactsRepository(db);
            var result = await repo.InsertAsync(contact);
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void InsertAsync_IfExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var contact = ConatactFaker.Generate();

            await db.Contacts.AddAsync(contact);
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();

            //Act
            var repo = new ContactsRepository(db);

            //Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.InsertAsync(contact));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void UpdateAsync_IfExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var contact = ConatactFaker.Generate();
            var contact2 = ConatactFaker.Generate();

            contact2.Id = contact.Id;

            await db.Contacts.AddAsync(contact);
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();

            //Act
            var repo = new ContactsRepository(db);
            var result = await repo.UpdateAsync(contact2.Id.ToString(), contact);
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void UpdateAsync_IfNotExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var contact = ConatactFaker.Generate();

            //Act
            var repo = new ContactsRepository(db);

            //Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.UpdateAsync(contact.Id.ToString(), contact));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_IfNotExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var contact = ConatactFaker.Generate();

            //Act
            var repo = new ContactsRepository(db);

            //Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.DeleteAsync(contact.Id.ToString()));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void DeleteAsync_IfExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var contact = ConatactFaker.Generate();

            await db.Contacts.AddAsync(contact);
            await db.SaveChangesAsync();

            //Act
            var repo = new ContactsRepository(db);
            var result = await repo.DeleteAsync(contact.Id.ToString());
            await db.SaveChangesAsync();

            //Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_IfExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var contacts = ConatactFaker.Generate(5);

            await db.Contacts.AddRangeAsync(contacts);
            await db.SaveChangesAsync();

            //Act
            var repo = new ContactsRepository(db);
            var result = await repo.GetAllAsync();

            //Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetAllAsync_IfNotExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            //Act
            var repo = new ContactsRepository(db);

            //Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.GetAllAsync());

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetByIdAsync_IfExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var contact = ConatactFaker.Generate();

            await db.Contacts.AddAsync(contact);
            await db.SaveChangesAsync();

            //Act
            var repo = new ContactsRepository(db);
            var result = await repo.GetByIdAsync(contact.Id.ToString());

            //Assert
            Assert.NotNull(result);

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }

        [Fact]
        public async void GetByIdAsync_IfNotExists()
        {
            //Arrange
            var db = TestDBContext.GetTestDBContext();

            var contact = ConatactFaker.Generate();

            //Act
            var repo = new ContactsRepository(db);

            //Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await repo.GetByIdAsync(contact.Id.ToString()));

            await db.Database.EnsureDeletedAsync();
            await db.DisposeAsync();
        }
    }
}
