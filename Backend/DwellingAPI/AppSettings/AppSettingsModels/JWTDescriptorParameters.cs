namespace DwellingAPI.AppSettings.AppSettingsModels
{
    public class JWTDescriptorParameters
    {
        public string Key { get; set; } = string.Empty;
        public int ExpiresInDays { get; set; }
    }
}
