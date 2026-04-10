using FlowdeskTaskboardApi.Models.ViewModels.Auth;
using FlowdeskTaskboardApi.Models.ViewModels.Auth.FlowdeskTaskboardApi.Models.ViewModels.Auth;

namespace FlowdeskTaskboardApi.Interface
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterViewModel model);
        Task<string> LoginAsync(LoginViewModel model);
        Task<UserViewModel?> GetUserByIdAsync(string id);
        Task<List<UserViewModel>> GetAllUsersAsync();
    }
}
