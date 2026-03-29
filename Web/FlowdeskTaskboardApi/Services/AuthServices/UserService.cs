using FlowdeskTaskboardApi.Helper;
using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels.Auth;
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

    //Register User
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

            // Validate role
            var role = string.IsNullOrWhiteSpace(model.Role) ? Roles.User : model.Role;

            if (!Roles.AllRoles.Contains(role))
                throw new Exception("Invalid role");

            await _userManager.AddToRoleAsync(user, role);

            // Log registration success
            await _logService.LogAsync("Information", $"User registered: {user.UserName}, Role: {role}", source: "RegisterAsync");
            _logger.LogInformation("User registered: {Username}, Role: {Role}", user.UserName, role);

            return "User registered successfully";
        }
        catch (Exception ex)
        {
            await _errorService.SaveErrorAsync(ex, "RegisterAsync");
            _logger.LogError(ex, "Error registering user {Username}", model.Username);
            throw;
        }
    }

    //Login User
    public async Task<string> LoginAsync(LoginViewModel model)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                throw new Exception( "Invalid credentials");

            // Generate Token
            var token = await _jwtService.GenerateTokenAsync(user, _userManager);

            // Log login success
            await _logService.LogAsync("Information", $"User logged in: {user.UserName}", source: "LoginAsync");
            _logger.LogInformation("User logged in: {Username}", user.UserName);

            return token;
        }
        catch (Exception ex)
        {
            await _errorService.SaveErrorAsync(ex, "LoginAsync");
            _logger.LogError(ex, "Error logging in user {Username}", model.Username);
            throw;
        }
    }
}