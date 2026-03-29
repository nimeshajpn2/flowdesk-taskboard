using FlowdeskTaskboardApi.Helper;
using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels.Auth;
using FlowdeskTaskboardApi.Models.ViewModels.Auth.FlowdeskTaskboardApi.Models.ViewModels.Auth;
using FlowdeskTaskboardApi.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtService _jwtService;
    private readonly ILogService _logService;
    private readonly IErrorService _errorService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        UserManager<IdentityUser> userManager,
        JwtService jwtService,
        ILogService logService,
        IErrorService errorService,
        ILogger<UserService> logger)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _logService = logService;
        _errorService = errorService;
        _logger = logger;
    }

    //User Register
    public async Task<string> RegisterAsync(RegisterViewModel model)
    {
        try
        {
            var user = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            var role = string.IsNullOrWhiteSpace(model.Role) ? Roles.User : model.Role;

            if (!Roles.AllRoles.Contains(role))
                throw new Exception(ErrorMessages.Auth.InvalidRole);

            await _userManager.AddToRoleAsync(user, role);

            await _logService.LogAsync(
                CommonConstants.LogLevelInformation,
                string.Format(LogMessages.Auth.UserRegistered, user.UserName, role),
                CommonConstants.Register);

            _logger.LogInformation(LogMessages.Auth.UserRegistered, user.UserName, role);

            return ResponseMessages.Auth.UserRegistered;
        }
        catch (Exception ex)
        {
            await _errorService.SaveErrorAsync(ex, CommonConstants.Register);
            _logger.LogError(ex, LogMessages.ErrorLogs.RegisterError, model.Username);
            throw;
        }
    }

    //User Login
    public async Task<string> LoginAsync(LoginViewModel model)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if(user is null)
                throw new UnauthorizedAccessException(ErrorMessages.Auth.InvalidUser);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                throw new UnauthorizedAccessException(ErrorMessages.Auth.InvalidPassword);

            var token = await _jwtService.GenerateTokenAsync(user, _userManager);

            await _logService.LogAsync(
                CommonConstants.LogLevelInformation,
                string.Format(LogMessages.Auth.UserLoggedIn, user.UserName),null,
                CommonConstants.Login);

            _logger.LogInformation(LogMessages.Auth.UserLoggedIn, user.UserName);

            return token;
        }
        catch (Exception ex)
        {
            await _errorService.SaveErrorAsync(ex, CommonConstants.Login);
            _logger.LogError(ex, LogMessages.ErrorLogs.LoginError, model.Username);
            throw;
        }
    }

    //Get user info by Id
    public async Task<UserViewModel?> GetByIdAsync(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                Role = roles.FirstOrDefault() ?? ""
            };
        }
        catch (Exception ex)
        {
            await _errorService.SaveErrorAsync(ex, "GetByIdAsync");
            _logger.LogError(ex, "Error fetching user info by Id {UserId}", id);
            throw;
        }
    }
}