using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Interfaces;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.Services.Interfaces
{
    public interface IApartmentService : IService<ApartmentModel>
    {
        Task<ResponseWrapper<ApartmentModel>> InsertAsync(ApartmentModel model);
        Task<ResponseWrapper<ApartmentPhotoModel>> AddPhotoAsync(ApartmentPhotoModel photoInfo);
        Task<ResponseWrapper<ApartmentModel>> AddMainPhotoAsync(ApartmentPhotoModel photoFile);
        Task<ResponseWrapper<ApartmentModel>> GetAllPhotosAsync(string apartmentId);
        Task<ResponseWrapper<ApartmentModel>> GetByIdAsync(string id);
        Task<ResponseWrapper<ApartmentModel>> DeleteMainPhotoAsync(string apartmentId);
        Task<ResponseWrapper<ApartmentPhotoModel>> DeletePhotoAsync(string apartmentId, string photoId);
        Task<ResponseWrapper<IEnumerable<ApartmentModel>>> GetAllByOrderRequirementsAsync(string orderId);
        Task<ResponseWrapper<ApartmentModel>> DeleteApartmentFromAllOrdersAsync(string apartmentId);
        Task<ResponseWrapper<IEnumerable<ApartmentModel>>> GetAllAsync();
    }
}
