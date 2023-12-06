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
        Task<ResponseWrapper<ApartmentPhoto>> AddPhotoAsync(ApartmentPhoto photoInfo, IFormFile photoFile);
        Task<ResponseWrapper<Apartment>> AddMainPhotoAsync(string apartmentId, IFormFile photoFile);
        Task<ResponseWrapper<Apartment>> GetAllPhotosAsync(string apartmentId);
        Task<ResponseWrapper<IEnumerable<Apartment>>> GetAllByAccountIdAsync(string accountId);

        Task<ResponseWrapper<Apartment>> DeleteMainPhotoAsync(string apartmentId);
        Task<ResponseWrapper<Apartment>> DeletePhotoAsync(string apartmentId, string photoId);

        Task<ResponseWrapper<IEnumerable<Apartment>>> GetAllByOrderRequirementsAsync(string orderId);
    }
}
