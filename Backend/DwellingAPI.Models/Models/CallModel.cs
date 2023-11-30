namespace DwellingAPI.Shared.Models
{
    public class CallModel
    {
        public string Id { get; set; }
        public string ToName { get; set; } = string.Empty;
        public string ToPhone { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public DateTime LastlyUpdatedDate { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
