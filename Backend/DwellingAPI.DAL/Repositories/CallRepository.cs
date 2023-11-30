using DwellingAPI.DAL.DBContext;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Interfaces;
using DwellingAPI.ResponseWrapper.Implementation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Repositories
{
    public class CallRepository : ICallRepository
    {
        private readonly AppDBContext _db;

        public CallRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<ResponseWrapper<Call>> DeleteAsync(string id)
        {
            try
            {
                var call = await _db.Calls.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));
                if(call == null)
                {
                    var errors = new List<string>()
                    {
                        new string("Call request is not found")
                    };
                    return new ResponseWrapper<Call>(errors);
                }
                _db.Calls.Remove(call);
                return new ResponseWrapper<Call>(call);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<Call>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<Call>>> GetAllAsync()
        {
            try
            {
                var calls = await _db.Calls.AsNoTracking().ToListAsync();
                if(calls.Count == 0)
                {
                    var errors = new List<string>()
                    {
                        new string("List is empty")
                    };
                    return new ResponseWrapper<IEnumerable<Call>>(errors);
                }
                return new ResponseWrapper<IEnumerable<Call>>(calls);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<IEnumerable<Call>>(errors);
            }
        }

        public async Task<ResponseWrapper<Call>> GetByIdAsync(string id)
        {
            try
            {
                var call = await _db.Calls.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));
                if (call == null)
                {
                    var errors = new List<string>()
                    {
                        new string("Call request is not found")
                    };
                    return new ResponseWrapper<Call>(errors);
                }
                return new ResponseWrapper<Call>(call);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<Call>(errors);
            }
        }

        public async Task<ResponseWrapper<Call>> InsertAsync(Call entity)
        {
            try
            {
                entity.CreationDate = DateTime.Now;
                entity.LastlyUpdatedDate = DateTime.Now;

                var requestedCall = await _db.Calls.AddAsync(entity);

                return new ResponseWrapper<Call>(requestedCall.Entity);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<Call>(errors);
            }
        }

        public async Task<ResponseWrapper<Call>> UpdateAsync(string id, Call entity)
        {
            try
            {
                var entityToUpdate = await _db.Calls.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));
                if (entityToUpdate != null)
                {
                    entity.LastlyUpdatedDate = DateTime.Now;
                    _db.Calls.Update(entity);
                    return new ResponseWrapper<Call>(entity);
                }
                else
                {
                    var errors = new List<string>()
                    {
                        new string("Requested item cannot be updated due to it`s absense")
                    };
                    return new ResponseWrapper<Call>(errors);
                }
            }
            catch(Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<Call>(errors);
            }
        }
    }
}
