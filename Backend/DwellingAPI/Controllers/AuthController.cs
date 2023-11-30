
using DwellingAPI.Authentication;
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
        private readonly AuthenticationProvider _authProvider;

        public AuthController(AuthenticationProvider authProvider)
        {
            _authProvider = authProvider;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<ResponseWrapper<AuthorizedUser>>> SignUpAsync([FromBody] SignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseWrapper<AuthorizedUser>(ModelState.Select(x=>x.Value).SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage)));
            }
            return Ok(await _authProvider.SignUpAndAuthorizeAsync(model));
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult<ResponseWrapper<AuthorizedUser>>> LogInAsync([FromBody] LogInModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResponseWrapper<AuthorizedUser>(ModelState.Select(x => x.Value).SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
            }
            return Ok(await _authProvider.LogInAndAuthorizeAsync(model));
        }

        [Authorize]
        [HttpGet("LogOut")]
        public async Task<ActionResult<ResponseWrapper<AuthorizedUser>>> LogOutAsync()
        {
            return Ok(await _authProvider.LogOutAsync());
        }
    }
}
