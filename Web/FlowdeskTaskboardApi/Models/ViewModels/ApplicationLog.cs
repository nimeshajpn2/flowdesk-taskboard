namespace FlowdeskTaskboardApi.Models.ViewModels
{
    public class ApplicationLog
    {
        public int Id { get; set; }

        public string? Level { get; set; }           // Info, Warning, Error
        public string? Message { get; set; }
        public string? Exception { get; set; }

        public string? Source { get; set; }         // Class/Service name
        public string? UserId { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
