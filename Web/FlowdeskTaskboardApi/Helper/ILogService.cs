namespace FlowdeskTaskboardApi.Helper
{
    public interface ILogService
    {
        Task LogAsync(string level, string message, string? exception = null, string? source = null);
    }
}
