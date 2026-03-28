namespace FlowdeskTaskboardApi.Models.ViewModels
{
    public class ErrorLog
    {
        public int Id { get; set; }

        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? Source { get; set; }
        public string? Path { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
