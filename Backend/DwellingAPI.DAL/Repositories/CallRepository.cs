using DwellingAPI.DAL.DBContext;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Exceptions;
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

        public async Task<Call> DeleteAsync(string id)
        {
            var call = await _db.Calls.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (call == null)
                throw new OperationFailedException("Call request is not found");

            return _db.Calls.Remove(call).Entity;
        }

        public async Task<IEnumerable<Call>> GetAllAsync()
        {
            var calls = await _db.Calls.AsNoTracking().ToListAsync();

            if (calls.Count == 0)
                throw new OperationFailedException("No call requests were found");

            return calls;
        }

        public async Task<Call> GetByIdAsync(string id)
        {
            var call = await _db.Calls.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (call == null)
                throw new OperationFailedException("Call request is not found");

            return call;
        }

        public async Task<Call> InsertAsync(Call entity)
        {
            entity.CreationDate = DateTime.Now;
            entity.LastlyUpdatedDate = DateTime.Now;

            var requestedCall = await _db.Calls.AddAsync(entity);

            return requestedCall.Entity;
        }

        public async Task<Call> UpdateAsync(string id, Call entity)
        {
            var entityToUpdate = await _db.Calls.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (entityToUpdate == null)
                throw new OperationFailedException("Call request is not found");

            entity.LastlyUpdatedDate = DateTime.Now;

            return _db.Calls.Update(entity).Entity;
        }
    }
}
