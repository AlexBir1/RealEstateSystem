namespace DwellingAPI.StartupSettings
{
    public interface IApplicationStartup
    {
        Task<bool> CreateDefaultRoles();
        Task<bool> CreateDefaultAdmin();
    }
}
