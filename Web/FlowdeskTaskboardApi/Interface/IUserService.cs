using FlowdeskTaskboardApi.Models.ViewModels.Auth;

namespace FlowdeskTaskboardApi.Interface
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterViewModel model);
        Task<string> LoginAsync(LoginViewModel model);
    }
}
