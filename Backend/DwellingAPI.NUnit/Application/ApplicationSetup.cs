using DwellingAPI.AppSettings;
using DwellingAPI.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.NUnit.Application
{
    public static class ApplicationSetup
    {
        public static TokenModel GetTestToken(AccountModel model)
        {
            AppSettingsProvider _appSettings = new AppSettingsProvider(InitConfiguration());

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
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = credetials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var JWT = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenModel
            {
                Token = tokenHandler.WriteToken(JWT),
                ExpirationDate = DateTime.Now.AddMinutes(1)
            };

        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }
    }
}
