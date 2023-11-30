using DwellingAPI.DAL.DBContext;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Interfaces;
using DwellingAPI.ResponseWrapper.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Repositories
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly AppDBContext _db;

        public ApartmentRepository(AppDBContext db)
        {
            _db = db;
        }

        public async Task<ResponseWrapper<Apartment>> AddMainPhotoAsync(string apartmentId, IFormFile photoFile)
        {
            try
            {
                string[] allowedExtensions = { "jpg, jpeg, png" };

                if (allowedExtensions.Any(x => x == photoFile.FileName.Split('.').Last()))
                {
                    var errors = new List<string>()
                    {
                        new string("Not allowed extension")
                    };
                    return new ResponseWrapper<Apartment>(errors);
                }

                var apartment = await _db.Apartments.SingleOrDefaultAsync(x => x.Id == Guid.Parse(apartmentId));

                if(apartment == null)
                {
                    var errors = new List<string>()
                    {
                        new string("Apartment is not found")
                    };
                    return new ResponseWrapper<Apartment>(errors);
                }

                var currentDirectory = Directory.GetCurrentDirectory();
                var directory = Directory.CreateDirectory(Path.Combine(currentDirectory, "Photos", apartment.Id.ToString(), "main"));

                using (var fileStream = new FileStream(Path.Combine(directory.FullName, photoFile.FileName), FileMode.CreateNew))
                {
                    photoFile.CopyTo(fileStream);
                    apartment.ImageUrl = "Photos/" + apartmentId + "/main/" + fileStream.Name.Split('\\').Last();
                }

                return new ResponseWrapper<Apartment>(apartment);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<Apartment>(errors);
            }
        }

        public async Task<ResponseWrapper<ApartmentPhoto>> AddPhotoAsync(ApartmentPhoto photoInfo, IFormFile photoFile)
        {
            try
            {
                string[] allowedExtensions = { "jpg, jpeg, png" };

                if (!allowedExtensions.Any(x => x.Contains(photoFile.FileName.Split('.').Last())))
                {
                    var errors = new List<string>()
                    {
                        new string("Not allowed extension")
                    };
                    return new ResponseWrapper<ApartmentPhoto>(errors);
                }

                var currentDirectory = Directory.GetCurrentDirectory();
                var directory = Directory.CreateDirectory(Path.Combine(currentDirectory, "Photos", photoInfo.ApartmentId.ToString()));

                using (var fileStream = new FileStream(Path.Combine(directory.FullName, photoFile.FileName), FileMode.Create))
                {
                    photoFile.CopyTo(fileStream);
                    
                    photoInfo.ImageUrl = "Photos/" + photoInfo.ApartmentId.ToString() + '/' + fileStream.Name.Split('\\').Last();
                }

                var result = await _db.ApartmentPhotos.AddAsync(photoInfo);

                return new ResponseWrapper<ApartmentPhoto>(photoInfo);
            }
            catch(Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };
                return new ResponseWrapper<ApartmentPhoto>(errors);
            }
        }

        public async Task<ResponseWrapper<Apartment>> DeleteAsync(string apartmentId)
        {
            try
            {
                var apartmentToDelete = await _db.Apartments.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(apartmentId));

                if (apartmentToDelete != null)
                {
                    _db.Apartments.Remove(apartmentToDelete);
                    return new ResponseWrapper<Apartment>(apartmentToDelete);
                }

                else
                {
                    var errors = new List<string>()
                    {
                        new string("Requested item is not found")
                    };
                    return new ResponseWrapper<Apartment>(errors);
                }
            }
            catch(Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Apartment>(errors);
            }
        }

        public async Task<ResponseWrapper<Apartment>> DeleteMainPhotoAsync(string apartmentId)
        {
            try
            {
                var apartment = await _db.Apartments.SingleOrDefaultAsync(x => x.Id == Guid.Parse(apartmentId));

                if (apartment == null)
                    return new ResponseWrapper<Apartment>(new List<string> { new string("No main photo.") });

                var currentDirectory = Directory.GetCurrentDirectory();
                DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(currentDirectory, "Photos", apartmentId.ToString(), "main"));
                FileInfo[] files = directoryInfo.GetFiles();

                var file = files.SingleOrDefault(x => x.Name == apartment.ImageUrl.Split('/').Last());

                apartment.ImageUrl = string.Empty;

                if (file == null)
                {
                    _db.Apartments.Update(apartment);
                    return new ResponseWrapper<Apartment>(apartment);
                }

                _db.Apartments.Update(apartment);

                file.Delete();

                return new ResponseWrapper<Apartment>(apartment);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Apartment>(errors);
            }
        }

        public async Task<ResponseWrapper<Apartment>> DeletePhotoAsync(string apartmentId, string photoId)
        {
            try
            {
                var apartmentPhoto = await _db.ApartmentPhotos.Include(x=>x.Apartment).AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(photoId));

                if (apartmentPhoto == null)
                    return new ResponseWrapper<Apartment>(new List<string> { new string("No such photo.") });

                var currentDirectory = Directory.GetCurrentDirectory();
                DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(currentDirectory, "Photos", apartmentId.ToString()));
                FileInfo[] files = directoryInfo.GetFiles();

                var file = files.SingleOrDefault(x => x.Name == apartmentPhoto.ImageUrl.Split('\\').Last());

                if(file == null)
                {
                    _db.ApartmentPhotos.Remove(apartmentPhoto);
                    return new ResponseWrapper<Apartment>(apartmentPhoto.Apartment);
                }

                _db.ApartmentPhotos.Remove(apartmentPhoto);

                file.Delete();

                return new ResponseWrapper<Apartment>(apartmentPhoto.Apartment);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Apartment>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<Apartment>>> GetAllAsync()
        {
            try
            {
                var apartments = await _db.Apartments.AsNoTracking().ToListAsync();

                if (apartments.Count > 0)
                    return new ResponseWrapper<IEnumerable<Apartment>>(apartments);

                return new ResponseWrapper<IEnumerable<Apartment>>(new List<string>() { new string("List is empty") });
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<IEnumerable<Apartment>>(errors);
            }
        }

        public async Task<ResponseWrapper<IEnumerable<Apartment>>> GetAllByAccountIdAsync(string accountId)
        {
            try
            {
                var apartments = await _db.OrdersApartments
                    .Where(x=>x.Order.AccountId == accountId)
                    .Include(x=>x.Apartment)
                    .Select(x=>x.Apartment)
                    .ToListAsync();

                if (apartments.Count > 0)
                    return new ResponseWrapper<IEnumerable<Apartment>>(apartments);

                return new ResponseWrapper<IEnumerable<Apartment>>(new List<string>() { new string("List is empty") });
            }
            catch(Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<IEnumerable<Apartment>>(errors);
            }
        }

        public async Task<ResponseWrapper<Apartment>> GetAllPhotosAsync(string apartmentId)
        {
            try
            {
                var apartment = await _db.Apartments.Where(x => x.Id == Guid.Parse(apartmentId)).Include(x => x.Photos).AsNoTracking().SingleOrDefaultAsync();

                if (apartment != null)
                    return new ResponseWrapper<Apartment>(apartment);

                var errors = new List<string>()
                {
                    new string("Requested item is not found")
                };

                return new ResponseWrapper<Apartment>(errors);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Apartment>(errors);
            }
        }

        public async Task<ResponseWrapper<Apartment>> GetByIdAsync(string id)
        {
            try
            {
                var apartment = await _db.Apartments.Include(x=>x.Details).Include(x=>x.Photos).AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if(apartment != null)
                    return new ResponseWrapper<Apartment>(apartment);

                var errors = new List<string>()
                {
                    new string("Requested item is not found")
                };

                return new ResponseWrapper<Apartment>(errors);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Apartment>(errors);
            }
        }

        public async Task<ResponseWrapper<Apartment>> InsertAsync(Apartment entity)
        {
            try
            {
                entity.Details!.CreationDate = DateTime.Now;
                entity.Details!.LastlyUpdatedDate = DateTime.Now;

                var result = await _db.Apartments.AddAsync(entity);
                return new ResponseWrapper<Apartment>(result.Entity);
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Apartment>(errors);
            }
        }

        public async Task<ResponseWrapper<Apartment>> UpdateAsync(string apartmentId, Apartment entity)
        {
            try
            {
                var entityToUpdate = await _db.Apartments.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(apartmentId));
                if(entityToUpdate != null)
                {
                    entity.Details!.LastlyUpdatedDate = DateTime.Now;
                    _db.Apartments.Update(entity);
                    return new ResponseWrapper<Apartment>(entity);
                }
                else
                {
                    var errors = new List<string>()
                    {
                        new string("Requested item cannot be updated due to it`s absense")
                    };
                    return new ResponseWrapper<Apartment>(errors);
                }
            }
            catch (Exception ex)
            {
                var errors = new List<string>()
                {
                    new string(ex.Message),
                    ex.InnerException != null ? new string(ex.InnerException?.Message) : string.Empty,
                };

                return new ResponseWrapper<Apartment>(errors);
            }
        }
    }
}
