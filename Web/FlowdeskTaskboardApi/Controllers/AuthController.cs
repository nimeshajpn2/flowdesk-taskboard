using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    //User Register
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var result = await _userService.RegisterAsync(model);
        return Ok(result);
    }

    //User Login
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var token = await _userService.LoginAsync(model);

        // Set JWT as HttpOnly cookie
        Response.Cookies.Append("jwtToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true, 
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1)
        });

        return Ok(new { Message = "Login successful" });
    }
}