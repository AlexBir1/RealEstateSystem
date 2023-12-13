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
    public class ContactsRepository : IContactsRepository
    {
        private readonly AppDBContext _db;

        public ContactsRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<Contact> DeleteAsync(string id)
        {
            var contact = await _db.Contacts.SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (contact == null)
                throw new OperationFailedException("Contact is not found");

            return _db.Contacts.Remove(contact).Entity;
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            var contacts = await _db.Contacts.ToListAsync();

            if (contacts.IsNullOrEmpty())
                throw new OperationFailedException("No contacts were found");

            return contacts;
        }

        public async Task<Contact> GetByIdAsync(string id)
        {
            var contact = await _db.Contacts.SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (contact is null)
                throw new OperationFailedException("Contact is not found");

            return contact;
        }

        public async Task<Contact> InsertAsync(Contact entity)
        {
            var contact = await _db.Contacts.SingleOrDefaultAsync(x => x.ContactOptionValue == entity.ContactOptionValue);

            if (contact != null)
                throw new OperationFailedException("Contact already exists");

            entity.CreationDate = DateTime.Now;
            entity.LastlyUpdatedDate = DateTime.Now;

            var result = await _db.Contacts.AddAsync(entity);

            return result.Entity;
        }

        public async Task<Contact> UpdateAsync(string id, Contact entity)
        {
            var contact = await _db.Contacts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (contact == null)
                throw new OperationFailedException("Contact is not found");

            entity.LastlyUpdatedDate = DateTime.Now;

            return _db.Contacts.Update(entity).Entity;
        }
    }
}
