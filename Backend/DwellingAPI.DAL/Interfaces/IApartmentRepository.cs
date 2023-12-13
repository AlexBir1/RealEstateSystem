using DwellingAPI.DAL.Entities;
using DwellingAPI.ResponseWrapper.Implementation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.DAL.Interfaces
{
    public interface IApartmentRepository : IRepository<Apartment>
    {
        Task<ApartmentPhoto> AddPhotoAsync(ApartmentPhoto photoInfo, IFormFile photoFile);
        Task<Apartment> AddMainPhotoAsync(string apartmentId, IFormFile photoFile);
        Task<Apartment> GetAllPhotosAsync(string apartmentId);
        Task<IEnumerable<Apartment>> GetAllByAccountIdAsync(string accountId);

        Task<Apartment> DeleteMainPhotoAsync(string apartmentId);
        Task<ApartmentPhoto> DeletePhotoAsync(string apartmentId, string photoId);

        Task<IEnumerable<Apartment>> GetAllByOrderRequirementsAsync(string orderId);
        Task<Apartment> DeleteApartmentFromAllOrdersAsync(string apartmentId);
    }
}
