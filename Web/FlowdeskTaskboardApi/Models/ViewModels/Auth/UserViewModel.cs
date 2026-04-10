using Microsoft.AspNetCore.Identity;

namespace FlowdeskTaskboardApi.Models.ViewModels.Auth
{
    namespace FlowdeskTaskboardApi.Models.ViewModels.Auth
    {
        public class UserViewModel
        {
            public string Id { get; set; } = default!;
            public string UserName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string? FullName { get; set; }
            public string? PhoneNumber { get; set; }
            public string Role { get; set; } = string.Empty;
        }
    }
}
