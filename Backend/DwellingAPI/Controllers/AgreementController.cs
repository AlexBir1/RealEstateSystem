using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Services.UOW;
using DwellingAPI.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace DwellingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AgreementController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;

        public AgreementController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseWrapper<AgreementModel>>> UpdateAsync(string id, [FromBody] AgreementModel model)
        {
            return Ok(await _serviceRepo.AgreementService.UpdateAsync(id, model));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseWrapper<AgreementModel>>> DeleteAsync(string id)
        {
            return Ok(await _serviceRepo.AgreementService.DeleteAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseWrapper<AgreementModel>>> InsertAsync([FromBody] AgreementModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseWrapper<AgreementModel>(ModelState.Select(x => x.Value).SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
            }
            return Ok(await _serviceRepo.AgreementService.InsertAsync(model));
        }

        [HttpGet("{agreementId}")]
        public async Task<ActionResult<ResponseWrapper<AgreementModel>>> GetByIdAsync(string agreementId)
        {
            return Ok(await _serviceRepo.AgreementService.GetByIdAsync(agreementId));
        }

        [HttpGet]
        public async Task<ActionResult<ResponseWrapper<IEnumerable<AgreementModel>>>> GetAllAsync()
        {
            return Ok(await _serviceRepo.AgreementService.GetAllAsync());
        }

        [HttpGet("ByAccountId/{accountId}")]
        public async Task<ActionResult<ResponseWrapper<IEnumerable<AgreementModel>>>> GetAllByAccountIdAsync(string accountId)
        {
            return Ok(await _serviceRepo.AgreementService.GetAllAsync(accountId));
        }
    }
}
