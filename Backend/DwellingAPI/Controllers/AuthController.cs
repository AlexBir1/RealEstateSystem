
using DwellingAPI.Authorization;
using DwellingAPI.Filters;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DwellingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthorizationProvider _authProvider;

        public AuthController(AuthorizationProvider authProvider)
        {
            _authProvider = authProvider;
        }

        [HttpPut("RefreshAuthToken")]
        public async Task<ActionResult<ResponseWrapper<AuthorizedUser>>> RefreshAuthTokenAsync([FromBody] AuthorizedUser model)
        {
            return Ok(await _authProvider.HandleAuthorizationAsync(model));
        }

        [HttpPost("SignUp")]
        [ValidationFilter]
        public async Task<ActionResult<ResponseWrapper<AuthorizedUser>>> SignUpAsync([FromBody] SignUpModel model)
        {
            return Ok(await _authProvider.HandleAuthorizationAsync(model));
        }

        [HttpPost("LogIn")]
        [ValidationFilter]
        public async Task<ActionResult<ResponseWrapper<AuthorizedUser>>> LogInAsync([FromBody] LogInModel model)
        {
            return Ok(await _authProvider.HandleAuthorizationAsync(model));
        }

        [Authorize]
        [HttpGet("LogOut")]
        public async Task<ActionResult<ResponseWrapper<AuthorizedUser>>> LogOutAsync()
        {
            return Ok(await _authProvider.LogOutAsync());
        }
    }
}
