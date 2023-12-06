
using DwellingAPI.AppSettings;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Services.Interfaces;
using DwellingAPI.Services.UOW;
using DwellingAPI.Shared.Enums;
using DwellingAPI.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DwellingAPI.Authentication
{
    public class AuthenticationProvider
    {
        private readonly AppSettingsProvider _appSettings;
        private readonly IAccountService _accountService;

        public AuthenticationProvider(AppSettingsProvider appSettings, IAccountService accountService)
        {
            _appSettings = appSettings;
            _accountService = accountService;
        }

        public async Task<ResponseWrapper<AuthorizedUser>> SignUpAndAuthorizeAsync(SignUpModel model)
        {
            var result = await _accountService.InsertAsync(model);

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

        public async Task<ResponseWrapper<AuthorizedUser>> LogInAndAuthorizeAsync(LogInModel model)
        {
            var result = await _accountService.LogInAsync(model);

            if (result.Data is null)
                return new ResponseWrapper<AuthorizedUser>(result.Errors);

            var roleResult = await _accountService.GetRoleAsync(result.Data.Id);

            if(roleResult.Data is null)
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

        public async Task<ResponseWrapper<AuthorizedUser>> RefreshAuthTokenAsync(AuthorizedUser model)
        {
            var result = await _accountService.GetByIdAsync(model.UserId);

            if (result.Data is null)
                return new ResponseWrapper<AuthorizedUser>(result.Errors);

            var roleResult = await _accountService.GetRoleAsync(model.UserId);

            if (roleResult.Data is null)
                return new ResponseWrapper<AuthorizedUser>(roleResult.Errors);

            result.Data.Role = roleResult.Data;

            var tokenModel = CreateJWT(result.Data);


            return new ResponseWrapper<AuthorizedUser>(new AuthorizedUser
            {
                UserId = result.Data.Id.ToString(),
                JWT = tokenModel.Token,
                Role = roleResult.Data!.ToString(),
                TokenExpirationDate = tokenModel.ExpirationDate,
                KeepAuthorized = model.KeepAuthorized
            });
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