
using DwellingAPI.AppSettings;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Services.Interfaces;
using DwellingAPI.Services.UOW;
using DwellingAPI.Shared.Enums;
using DwellingAPI.Shared.Interfaces;
using DwellingAPI.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DwellingAPI.Authorization
{
    public class AuthorizationProvider
    {
        private readonly AppSettingsProvider _appSettings;
        private readonly IAccountService _accountService;

        public AuthorizationProvider(AppSettingsProvider appSettings, IAccountService accountService)
        {
            _appSettings = appSettings;
            _accountService = accountService;
        }

        public async Task<ResponseWrapper<AuthorizedUser>> HandleAuthorizationAsync(IAuthorizationModel model)
        {
            ResponseWrapper<AccountModel> result;

            if (model.GetType() == typeof(SignUpModel))
                result = await _accountService.InsertAsync((SignUpModel)model);

            else if (model.GetType() == typeof(LogInModel))
                result = await _accountService.LogInAsync((LogInModel)model);

            else if (model.GetType() == typeof(AuthorizedUser))
                result = await _accountService.GetByIdAsync(((AuthorizedUser)model).UserId);
            else
                return new ResponseWrapper<AuthorizedUser>(new List<string>() { new string("Invalid model type") });

            if (result.Data is null)
                return new ResponseWrapper<AuthorizedUser>(result.Errors);

            var roleResult = await _accountService.GetRoleAsync(result.Data.Id);

            if (roleResult.Data is null)
                return new ResponseWrapper<AuthorizedUser>(result.Errors);

            result.Data.Role = roleResult.Data;

            var tokenModel = CreateJWT(result.Data);

            var authorizedUser = new AuthorizedUser
            {
                UserId = result.Data.Id.ToString(),
                JWT = tokenModel.Token,
                Role = roleResult.Data!.ToString(),
                TokenExpirationDate = tokenModel.ExpirationDate,
                KeepAuthorized = model.KeepAuthorized
            };

            return new ResponseWrapper<AuthorizedUser>(authorizedUser);
        }

        public async Task<ResponseWrapper<AuthorizedUser>> LogOutAsync()
        {
            var result = await _accountService.LogOutAsync();

            if (result.Data is null)
                return new ResponseWrapper<AuthorizedUser>(result.Errors);

            return new ResponseWrapper<AuthorizedUser>(new AuthorizedUser
            {
                UserId = null,
                JWT = null,
                Role = null,
            });
        }

        private TokenModel CreateJWT(AccountModel model)
        {
            var defaultJWTDescriptorParameters = _appSettings.GetDefaultJWTDescriptorParameters();

            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, model.Id),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.MobilePhone, model.MobilePhone),
                new Claim(ClaimTypes.Role, model.Role)
            };

            var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(defaultJWTDescriptorParameters.Key));

            var credetials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(defaultJWTDescriptorParameters.ExpiresInDays),
                SigningCredentials = credetials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var JWT = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenModel
            {
                Token = tokenHandler.WriteToken(JWT),
                ExpirationDate = DateTime.Now.AddDays(defaultJWTDescriptorParameters.ExpiresInDays)
            };
        }
    }
}