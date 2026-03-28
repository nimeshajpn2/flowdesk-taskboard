namespace FlowdeskTaskboardApi.Helper
{
    public interface IErrorService
    {
        Task SaveErrorAsync(Exception ex, string path);
    }
}
