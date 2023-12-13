using DwellingAPI.DAL.DBContext;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Exceptions;
using DwellingAPI.DAL.Interfaces;
using DwellingAPI.ResponseWrapper.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<Apartment> AddMainPhotoAsync(string apartmentId, IFormFile photoFile)
        {
            var apartment = await _db.Apartments.SingleOrDefaultAsync(x => x.Id == Guid.Parse(apartmentId));

            if (apartment == null)
                throw new OperationFailedException("Apartment is not found");

            if (!IsFileExtensionAllowed(photoFile.FileName.Split('.').Last()))
                throw new OperationFailedException("Current file extension is not allowed");

            var currentDirectory = Directory.GetCurrentDirectory();
            var directory = Directory.CreateDirectory(Path.Combine(currentDirectory, "Photos", apartment.Id.ToString(), "main"));

            using (var fileStream = new FileStream(Path.Combine(directory.FullName, photoFile.FileName), FileMode.CreateNew))
            {
                photoFile.CopyTo(fileStream);
                apartment.ImageUrl = "Photos/" + apartmentId + "/main/" + fileStream.Name.Split('\\').Last();
            }

            return apartment;
        }

        public async Task<ApartmentPhoto> AddPhotoAsync(ApartmentPhoto photoInfo, IFormFile photoFile)
        {
            if (!IsFileExtensionAllowed(photoFile.FileName.Split('.').Last()))
                throw new OperationFailedException("Current file extension is not allowed");

            var currentDirectory = Directory.GetCurrentDirectory();
            var directory = Directory.CreateDirectory(Path.Combine(currentDirectory, "Photos", photoInfo.ApartmentId.ToString()));

            using (var fileStream = new FileStream(Path.Combine(directory.FullName, photoFile.FileName), FileMode.Create))
            {
                photoFile.CopyTo(fileStream);

                photoInfo.ImageUrl = "Photos/" + photoInfo.ApartmentId.ToString() + '/' + fileStream.Name.Split('\\').Last();
            }

            var result = await _db.ApartmentPhotos.AddAsync(photoInfo);

            return result.Entity;
        }

        public async Task<Apartment> DeleteAsync(string apartmentId)
        {
            var apartmentToDelete = await _db.Apartments.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(apartmentId));

            if (apartmentToDelete == null)
                throw new OperationFailedException("Apartment is not found");

            var result = _db.Apartments.Remove(apartmentToDelete);

            return result.Entity;
        }

        public async Task<Apartment> DeleteMainPhotoAsync(string apartmentId)
        {
            var apartment = await _db.Apartments.SingleOrDefaultAsync(x => x.Id == Guid.Parse(apartmentId));

            if (apartment == null)
                throw new OperationFailedException("Apartment is not found");

            var currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(currentDirectory, "Photos", apartmentId.ToString(), "main"));
            FileInfo[] files = directoryInfo.GetFiles();

            var file = files.SingleOrDefault(x => x.Name == apartment.ImageUrl.Split('/').Last());

            apartment.ImageUrl = string.Empty;

            if (file == null)
                return _db.Apartments.Update(apartment).Entity;

            file.Delete();

            return _db.Apartments.Update(apartment).Entity;
        }

        public async Task<ApartmentPhoto> DeletePhotoAsync(string apartmentId, string photoId)
        {
            var apartmentPhoto = await _db.ApartmentPhotos.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(photoId));

            if (apartmentPhoto == null)
                throw new OperationFailedException("Photo is not found");

            var currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(currentDirectory, "Photos", apartmentId.ToString()));
            FileInfo[] files = directoryInfo.GetFiles();

            var file = files.SingleOrDefault(x => x.Name == apartmentPhoto.ImageUrl.Split('/').Last());

            if (file == null)
                return _db.ApartmentPhotos.Remove(apartmentPhoto).Entity;

            file.Delete();

            return _db.ApartmentPhotos.Remove(apartmentPhoto).Entity;
        }

        public async Task<IEnumerable<Apartment>> GetAllAsync()
        {
            var apartments = await _db.Apartments.AsNoTracking().ToListAsync();

            if (apartments.Count == 0)
                throw new OperationFailedException("No apartments were found");

            return apartments;
        }

        public async Task<IEnumerable<Apartment>> GetAllByAccountIdAsync(string accountId)
        {
            var apartments = await _db.OrdersApartments
                .Where(x => x.Order.AccountId == accountId)
                .Include(x => x.Apartment)
                .Select(x => x.Apartment)
                .ToListAsync();

            if (apartments.Count == 0)
                throw new OperationFailedException("No apartments were found");

            return apartments;
        }

        public async Task<IEnumerable<Apartment>> GetAllByOrderRequirementsAsync(string orderId)
        {
            var order = await _db.Orders.SingleOrDefaultAsync(x => x.Id == Guid.Parse(orderId));

            if (order == null)
                throw new OperationFailedException("Order is not found");

            var apartments = await _db.Apartments.Where(x => x.Price <= order.EstimatedPriceLimit && x.Rooms == order.EstimatedRoomsQuantity && x.City == order.City).AsNoTracking().ToListAsync();

            if (apartments.Count() == 0)
                throw new OperationFailedException("No apartments were found");

            return apartments;
        }

        public async Task<Apartment> GetAllPhotosAsync(string apartmentId)
        {
            var apartment = await _db.Apartments.Where(x => x.Id == Guid.Parse(apartmentId)).Include(x => x.Photos).AsNoTracking().SingleOrDefaultAsync();

            if (apartment == null)
                throw new OperationFailedException("Apartment is not found");

            return apartment;
        }

        public async Task<Apartment> GetByIdAsync(string id)
        {
            var apartment = await _db.Apartments.Include(x => x.Details).Include(x => x.Photos).AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (apartment == null)
                throw new OperationFailedException("Apartment is not found");

            return apartment;
        }

        public async Task<Apartment> InsertAsync(Apartment entity)
        {
            entity.Details!.CreationDate = DateTime.Now;
            entity.Details!.LastlyUpdatedDate = DateTime.Now;

            if (_db.Apartments.SingleOrDefault(x => x.Number == entity.Number) != null)
                throw new OperationFailedException("Apartment already exists");

            var result = await _db.Apartments.AddAsync(entity);
            return result.Entity;
        }

        public async Task<Apartment> DeleteApartmentFromAllOrdersAsync(string apartmentId)
        {
            var apartment = await _db.Apartments.Include(x => x.ApartmentOrders).ThenInclude(x => x.Order).SingleOrDefaultAsync(x => x.Id == Guid.Parse(apartmentId));
            var orders = apartment.ApartmentOrders.Select(x => x.Order).DistinctBy(x => x.Id).ToList();

            orders.ForEach(x =>
            {
                x.OrderApartments = apartment.ApartmentOrders.Where(y => y.OrderId == x.Id && y.ApartmentId != Guid.Parse(apartmentId)).ToList() ?? new List<OrderApartment>();
            });

            if (apartment == null)
                throw new OperationFailedException("Apartment is not found");

            if (apartment.ApartmentOrders.Any())
                _db.OrdersApartments.RemoveRange(apartment.ApartmentOrders);

            orders.ForEach(x =>
            {
                if (x.OrderApartments.IsNullOrEmpty())
                {
                    x.OrderStatus = OrderStatus.SearchForApartment;
                    _db.Orders.Update(x);
                }
            });

            return apartment;
        }

        public async Task<Apartment> UpdateAsync(string apartmentId, Apartment entity)
        {
            var entityToUpdate = await _db.Apartments.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(apartmentId));

            if (entityToUpdate == null)
                throw new OperationFailedException("Apartment is not found");

            entity.Details!.LastlyUpdatedDate = DateTime.Now;

            return _db.Apartments.Update(entity).Entity;
        }

        private bool IsFileExtensionAllowed(string extension)
        {
            string[] allowedExtensions = { "jpg, jpeg, png" };

            if (allowedExtensions.Any(x => x == extension))
                return false;
            return true;
        }
    }
}
