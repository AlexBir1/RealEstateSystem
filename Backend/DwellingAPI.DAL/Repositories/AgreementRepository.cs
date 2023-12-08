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
    public class AgreementRepository : IAgreementRepository
    {
        private readonly AppDBContext _db;

        public AgreementRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<ResponseWrapper<Agreement>> DeleteAsync(string id)
        {
            try
            {
                var agreement = await _db.Agreements.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (agreement == null)
                    return new ResponseWrapper<Agreement>(new List<string> { new string("No such agreement.") });

                _db.Agreements.Remove(agreement);

                return new ResponseWrapper<Agreement>(agreement);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Agreement>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<Agreement>>> GetAllAsync()
        {
            try
            {
                var agreements = await _db.Agreements.AsNoTracking().ToListAsync();

                if (agreements.Count == 0)
                    return new ResponseWrapper<IEnumerable<Agreement>>(new List<string> { new string("List is empty") });

                return new ResponseWrapper<IEnumerable<Agreement>>(agreements);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<IEnumerable<Agreement>>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<Agreement>>> GetAllByAccountIdAsync(string accountId)
        {
            try
            {
                var agreements = await _db.Agreements.Where(x=>x.AccountId == accountId).AsNoTracking().ToListAsync();

                if (agreements.Count == 0)
                    return new ResponseWrapper<IEnumerable<Agreement>>(new List<string> { new string("List is empty") });

                return new ResponseWrapper<IEnumerable<Agreement>>(agreements);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<IEnumerable<Agreement>>(errors);
            }
        }

        public async Task<ResponseWrapper<Agreement>> GetByIdAsync(string id)
        {
            try
            {
                var agreement = await _db.Agreements.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (agreement == null)
                    return new ResponseWrapper<Agreement>(new List<string> { new string("No such agreement.") });

                return new ResponseWrapper<Agreement>(agreement);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Agreement>(errors);
            }
        }

        public async Task<ResponseWrapper<Agreement>> InsertAsync(Agreement entity)
        {
            try
            {
                var agreement = await _db.Agreements
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.ApartmentCity == entity.ApartmentCity && x.ApartmentAddress == entity.ApartmentAddress);

                if (agreement != null)
                    return new ResponseWrapper<Agreement>(new List<string> { new string("Agreement already exists") });

                entity.CreationDate = DateTime.Now;
                entity.LastlyUpdatedDate = DateTime.Now;

                var result = await _db.Agreements.AddAsync(entity);

                return new ResponseWrapper<Agreement>(result.Entity);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Agreement>(errors);
            }
        }

        public async Task<ResponseWrapper<Agreement>> UpdateAsync(string id, Agreement entity)
        {
            try
            {
                var agreement = await _db.Agreements.AsNoTracking().SingleOrDefaultAsync(x=>x.Id == Guid.Parse(id));

                if(agreement == null)
                    return new ResponseWrapper<Agreement>(new List<string> { new string("No such agreement.") });

                entity.LastlyUpdatedDate = DateTime.Now;

                var updatedEntity = _db.Agreements.Update(entity); 

                return new ResponseWrapper<Agreement>(updatedEntity.Entity);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Agreement>(errors);
            }
        }
    }
}
