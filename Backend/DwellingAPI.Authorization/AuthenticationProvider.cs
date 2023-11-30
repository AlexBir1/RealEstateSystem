
using DwellingAPI.AppSettings;
using DwellingAPI.ResponseWrapper.Implementation;
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
        private readonly AppSettingsProvider appSettings;

        public AuthenticationProvider(AppSettingsProvider appSettings)
        {
            this.appSettings = appSettings;
        }

        public Task<ResponseWrapper<AuthorizedUser>> SignUpAndAuthorize(SignUpModel model)
        {

        }

        public Task<ResponseWrapper<AuthorizedUser>> LogInAndAuthorize(LogInModel model)
        {

        }

        private string CreateJWT(AccountModel model)
        {
            try
            {
                var defaultJWTDescriptorParameters = appSettings.GetDefaultJWTDescriptorParameters();
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
                return tokenHandler.WriteToken(JWT);

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}