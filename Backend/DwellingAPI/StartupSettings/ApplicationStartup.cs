using DwellingAPI.AppSettings;
using DwellingAPI.Services.UOW;
using DwellingAPI.Shared.Models;

namespace DwellingAPI.StartupSettings
{
    public class ApplicationStartup : IApplicationStartup
    {
        private readonly IServiceRepository _serviceRepo;
        private readonly AppSettingsProvider _appSettings;

        public ApplicationStartup(IServiceRepository serviceRepo, AppSettingsProvider appSettings)
        {
            _serviceRepo = serviceRepo;
            _appSettings = appSettings;
        }

        public async Task<bool> CreateDefaultAdmin()
        {
            var defaultAdmin = _appSettings.GetDefaultAdmin();

            var adminResult = await _serviceRepo.AccountService.InsertAsync(new SignUpModel
            {
                FullName = defaultAdmin.Fullname,
                Username = defaultAdmin.Username,
                Password = defaultAdmin.Password,
                PasswordConfirm = defaultAdmin.Password,
                MobilePhone = defaultAdmin.MobilePhone,
                Email = defaultAdmin.Email,
            });

            if (adminResult.Data == null)
                return false;

            var defaultRoles = _appSettings.GetDefaultRoles();

            var adminRoleResult = await _serviceRepo.AccountService.SetRoleAsync(adminResult.Data.Id, defaultRoles.Admin);

            if(adminRoleResult.Data == null)
                return false;

            return true;
        }

        public async Task<bool> CreateDefaultRoles()
        {
            var defaultRoles = _appSettings.GetDefaultRoles();

            List<string> roles = new List<string>();

            foreach(var property in defaultRoles.GetType().GetProperties())
            {
                roles.Add(property.GetValue(defaultRoles)!.ToString()!);
            }

            var result = await _serviceRepo.RolesService.SetAvailableRoles(roles);

            if(result.Errors.Count() > 0)
                return false;

            return true;
        }
    }
}
