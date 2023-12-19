using DwellingAPI.Filters;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Services.UOW;
using DwellingAPI.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Data;

namespace DwellingAPI.Controllers
{
    [Route("api/[controller]")]
    public class ApartmentsController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;

        public ApartmentsController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseWrapper<IEnumerable<ApartmentModel>>>> GetApartmentsAsync()
        {
            return Ok(await _serviceRepo.ApartmentService.GetAllAsync());
        }

        [HttpGet("ByOrderRequirements/{orderId}")]
        public async Task<ActionResult<ResponseWrapper<IEnumerable<ApartmentModel>>>> GetApartmentsByOrderRequirementsAsync(string orderId)
        {
            return Ok(await _serviceRepo.ApartmentService.GetAllByOrderRequirementsAsync(orderId));
        }

        [HttpGet("{apartmentId}")]
        public async Task<ActionResult<ResponseWrapper<ApartmentModel>>> GetApartmentAsync(string apartmentId)
        {
            return Ok(await _serviceRepo.ApartmentService.GetByIdAsync(apartmentId));
        }

        [HttpPost]
        [Authorize(Roles = "Realtor, Admin")]
        [ValidationFilter]
        public async Task<ActionResult<ResponseWrapper<ApartmentModel>>> InsertApartment([FromBody] ApartmentModel model)
        {
            return Ok(await _serviceRepo.ApartmentService.InsertAsync(model));
        }

        [HttpPost("{apartmentId}/AddMainPhoto")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 2048576000)]
        [Authorize(Roles = "Realtor, Admin")]
        public async Task<ActionResult<ResponseWrapper<ApartmentModel>>> InsertApartmentMainPhoto(string apartmentId)
        {
            return Ok(await _serviceRepo.ApartmentService.AddMainPhotoAsync(new ApartmentPhotoModel { ApartmentId = Guid.Parse(apartmentId), PhotoFile = Request.Form.Files[0] }));
        }

        [HttpPost("{apartmentId}/AddPhoto")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 2048576000)]
        [Authorize(Roles = "Realtor, Admin")]
        public async Task<ActionResult<ResponseWrapper<ApartmentModel>>> InsertApartmentPhoto(string apartmentId)
        {
            return Ok(await _serviceRepo.ApartmentService.AddPhotoAsync(new ApartmentPhotoModel { ApartmentId = Guid.Parse(apartmentId), PhotoFile = Request.Form.Files[0] }));
        }

        [HttpPut("{apartmentId}")]
        [Authorize(Roles = "Realtor, Admin")]
        [ValidationFilter]
        public async Task<ActionResult<ResponseWrapper<ApartmentModel>>> UpdateApartment(string apartmentId, [FromBody] ApartmentModel model)
        {
            return Ok(await _serviceRepo.ApartmentService.UpdateAsync(apartmentId, model));
        }

        [HttpDelete("{apartmentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseWrapper<ApartmentModel>>> DeleteApartment(string apartmentId)
        {
            return Ok(await _serviceRepo.ApartmentService.DeleteAsync(apartmentId));
        }

        [HttpDelete("{apartmentId}/InAllOrders")]
        [Authorize]
        public async Task<ActionResult<ResponseWrapper<ApartmentModel>>> DeleteApartmentFromAllOrders(string apartmentId)
        {
            return Ok(await _serviceRepo.ApartmentService.DeleteApartmentFromAllOrdersAsync(apartmentId));
        }

        [HttpPut("{apartmentId}/DeleteMainPhoto")]
        [Authorize(Roles = "Realtor, Admin")]
        public async Task<ActionResult<ResponseWrapper<ApartmentModel>>> DeleteApartmentMainPhoto(string apartmentId)
        {
            return Ok(await _serviceRepo.ApartmentService.DeleteMainPhotoAsync(apartmentId));
        }

        [HttpPut("{apartmentId}/DeletePhoto/{photoId}")]
        [Authorize(Roles = "Realtor, Admin")]
        public async Task<ActionResult<ResponseWrapper<ApartmentPhotoModel>>> DeleteApartmentPhoto(string apartmentId, string photoId)
        {
            return Ok(await _serviceRepo.ApartmentService.DeletePhotoAsync(apartmentId, photoId));
        }
    }
}
