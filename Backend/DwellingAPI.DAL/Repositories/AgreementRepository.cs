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
    public class AgreementRepository : IAgreementRepository
    {
        private readonly AppDBContext _db;

        public AgreementRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<Agreement> DeleteAsync(string id)
        {
            var agreement = await _db.Agreements.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (agreement == null)
                throw new OperationFailedException("Agreement is not found");

            _db.Agreements.Remove(agreement);

            return agreement;
        }

        public async Task<IEnumerable<Agreement>> GetAllAsync()
        {

            var agreements = await _db.Agreements.AsNoTracking().ToListAsync();

            if (agreements.Count == 0)
                throw new OperationFailedException("No agreements were found");

            return agreements;
        }

        public async Task<IEnumerable<Agreement>> GetAllByAccountIdAsync(string accountId)
        {
            var agreements = await _db.Agreements.Where(x => x.AccountId == accountId).AsNoTracking().ToListAsync();

            if (agreements.Count == 0)
                throw new OperationFailedException("No agreements were found");

            return agreements;

        }

        public async Task<Agreement> GetByIdAsync(string id)
        {
            var agreement = await _db.Agreements.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (agreement == null)
                throw new OperationFailedException("Agreement is not found");

            return agreement;
        }

        public async Task<Agreement> InsertAsync(Agreement entity)
        {
            var agreement = await _db.Agreements
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApartmentCity == entity.ApartmentCity && x.ApartmentAddress == entity.ApartmentAddress);

            if (agreement != null)
                throw new OperationFailedException("Agreement already exists");

            entity.CreationDate = DateTime.Now;
            entity.LastlyUpdatedDate = DateTime.Now;

            var result = await _db.Agreements.AddAsync(entity);

            return result.Entity;
        }

        public async Task<Agreement> UpdateAsync(string id, Agreement entity)
        {
            var agreement = await _db.Agreements.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (agreement == null)
                throw new OperationFailedException("Agreement is not found");

            entity.LastlyUpdatedDate = DateTime.Now;

            var updatedEntity = _db.Agreements.Update(entity);

            return updatedEntity.Entity;
        }
    }
}
