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
    public class ContactsRepository : IContactsRepository
    {
        private readonly AppDBContext _db;

        public ContactsRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<ResponseWrapper<Contact>> DeleteAsync(string id)
        {
            try
            {
                var contact = await _db.Contacts.SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if(contact is null)
                    return new ResponseWrapper<Contact>(new List<string> { new string("Contact does not exist.")});

                _db.Contacts.Remove(contact);

                return new ResponseWrapper<Contact>(contact);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Contact>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<Contact>>> GetAllAsync()
        {
            try
            {
                var contacts = await _db.Contacts.ToListAsync();

                if(contacts.IsNullOrEmpty())
                    return new ResponseWrapper<IEnumerable<Contact>>(new List<string>() { new string("List is empty") });

                return new ResponseWrapper<IEnumerable<Contact>>(contacts);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<IEnumerable<Contact>>(errors);
            }
        }

        public async Task<ResponseWrapper<Contact>> GetByIdAsync(string id)
        {
            try
            {
                var contact = await _db.Contacts.SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (contact is null)
                    return new ResponseWrapper<Contact>(new List<string> { new string("Contact does not exist.") });

                return new ResponseWrapper<Contact>(contact);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Contact>(errors);
            }
        }

        public async Task<ResponseWrapper<Contact>> InsertAsync(Contact entity)
        {
            try
            {
                var contact = await _db.Contacts.SingleOrDefaultAsync(x => x.ContactOptionValue == entity.ContactOptionValue);

                if (contact is not null)
                    return new ResponseWrapper<Contact>(new List<string> { new string("Contact does exist.") });

                entity.CreationDate = DateTime.Now;
                entity.LastlyUpdatedDate = DateTime.Now;

                var result = await _db.Contacts.AddAsync(entity);

                return new ResponseWrapper<Contact>(result.Entity);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Contact>(errors);
            }
        }

        public async Task<ResponseWrapper<Contact>> UpdateAsync(string id, Contact entity)
        {
            try
            {
                var contact = await _db.Contacts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (contact is null)
                    return new ResponseWrapper<Contact>(new List<string> { new string("Contact does not exist.") });

                entity.LastlyUpdatedDate = DateTime.Now;

                _db.Contacts.Update(entity);

                return new ResponseWrapper<Contact>(contact);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Contact>(errors);
            }
        }
    }
}
