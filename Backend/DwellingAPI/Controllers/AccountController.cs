using DwellingAPI.Authentication;
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
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;

        public AccountController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        [HttpPut("{id}/ChangePassword")]
        public async Task<ActionResult<ResponseWrapper<AccountModel>>> ChangeAccountPassword(string id, [FromBody] ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseWrapper<AccountModel>(ModelState.Select(x => x.Value).SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
            }
            return Ok(await _serviceRepo.AccountService.ChangePasswordAsync(id, model));
        }

        [HttpPut("{accountId}")]
        public async Task<ActionResult<ResponseWrapper<AccountModel>>> UpdateAccount(string accountId, [FromBody] AccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseWrapper<AccountModel>(ModelState.Select(x => x.Value).SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
            }
            return Ok(await _serviceRepo.AccountService.UpdateAsync(accountId, model));
        }

        [HttpDelete("{accountId}")]
        public async Task<ActionResult<ResponseWrapper<AccountModel>>> DeleteAccount(string accountId)
        {
            return Ok(await _serviceRepo.AccountService.DeleteAsync(accountId));
        }

        [HttpGet("{accountId}")]
        public async Task<ActionResult<ResponseWrapper<AccountModel>>> GetAccountById(string accountId)
        {
            return Ok(await _serviceRepo.AccountService.GetByIdAsync(accountId));
        }

        [HttpGet]
        [Authorize(Roles = "Realtor, Admin")]
        public async Task<ActionResult<ResponseWrapper<IEnumerable<AccountModel>>>> GetAllAccounts()
        {
            return Ok(await _serviceRepo.AccountService.GetAllAsync());
        }
    }
}
