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
    public class CallController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;

        public CallController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseWrapper<CallModel>>> InsertCall([FromBody] RequestCallModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseWrapper<CallModel>(ModelState.Select(x => x.Value).SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
            }
            return Ok(await _serviceRepo.CallService.InsertAsync(model));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<ResponseWrapper<IEnumerable<CallModel>>>> GetCalls()
        {
            return Ok(await _serviceRepo.CallService.GetAllAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{callId}")]
        public async Task<ActionResult<ResponseWrapper<CallModel>>> DeleteCall(string callId)
        {
            return Ok(await _serviceRepo.CallService.DeleteAsync(callId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{callId}")]
        public async Task<ActionResult<ResponseWrapper<CallModel>>> UpdateCall(string callId, [FromBody] CallModel model)
        {
            return Ok(await _serviceRepo.CallService.UpdateAsync(callId, model));
        }
    }
}
