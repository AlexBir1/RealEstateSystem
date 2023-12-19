
using DwellingAPI.Filters;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Services.UOW;
using DwellingAPI.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DwellingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;

        public ContactsController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseWrapper<IEnumerable<ContactModel>>>> GetContacts()
        {
            return Ok(await _serviceRepo.ContactsService.GetContacts());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidationFilter]
        public async Task<ActionResult<ResponseWrapper<ContactModel>>> InsertContact(ContactModel model)
        {
            return Ok(await _serviceRepo.ContactsService.InsertContact(model));
        }


        [HttpDelete("{contactId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseWrapper<IEnumerable<ContactModel>>>> DeleteContact(string contactId)
        {
            return Ok(await _serviceRepo.ContactsService.DeleteContacts(contactId));
        }
    }
}
