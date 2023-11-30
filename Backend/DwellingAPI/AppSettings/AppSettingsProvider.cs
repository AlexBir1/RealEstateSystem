using DwellingAPI.AppSettings.AppSettingsModels;

namespace DwellingAPI.AppSettings
{
    public class AppSettingsProvider
    {
        private readonly IConfiguration _config;

        public AppSettingsProvider(IConfiguration config)
        {
            _config = config;
        }

        public DefaultAdmin GetDefaultAdmin()
        {
            return new DefaultAdmin()
            {
                Fullname = _config["DefaultAdmin:Fullname"]!.ToString(),
                Username = _config["DefaultAdmin:Username"]!.ToString(),
                Password = _config["DefaultAdmin:Password"]!.ToString(),
                MobilePhone = _config["DefaultAdmin:MobilePhone"]!.ToString(),
                Email = _config["DefaultAdmin:Email"]!.ToString(),
            };
        }

        public DefaultRoles GetDefaultRoles()
        {
            return new DefaultRoles()
            {
                User = _config["DefaultRoles:User"]!.ToString(),
                Realtor = _config["DefaultRoles:Realtor"]!.ToString(),
                Admin = _config["DefaultRoles:Admin"]!.ToString()
            };
        }

        public JWTDescriptorParameters GetDefaultJWTDescriptorParameters()
        {
            return new JWTDescriptorParameters()
            {
                Key = _config["JWT:Key"]!.ToString(),
                ExpiresInDays = Convert.ToInt32(_config["JWT:ExpiresInDays"]!),
            };
        }
    }
}
