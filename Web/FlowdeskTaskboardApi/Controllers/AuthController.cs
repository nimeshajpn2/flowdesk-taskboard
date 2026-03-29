using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels.Auth;
using FlowdeskTaskboardApi.Shared;
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
        try
        {
            var result = await _userService.RegisterAsync(model);
            return Ok(new { Message = ResponseMessages.Auth.RegisterSuccess, Data = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    //User Login
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        try
        {
            var token = await _userService.LoginAsync(model);

            // Set JWT as HttpOnly cookie
            Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return Ok(new { Message = ResponseMessages.Auth.LoginSuccess,});
        }
        catch (Exception ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        try
        {
            // ClaimTypes.NameIdentifier 
            var userIdClaim = User.Claims
                .FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { Message = ResponseMessages.Auth.UserNotLoggedin }); 

            // userIdClaim is usually a GUID string in Identity
            var user = await _userService.GetByIdAsync(userIdClaim);

            if (user is null)
                return NotFound(new { Message = ResponseMessages.Auth.UserNotFound }); 

            return Ok(new { Message = ResponseMessages.Auth.UserInfoFetched, Data = user });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // Clear the JWT cookie
        Response.Cookies.Append("jwtToken", "", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(-1) // expire it immediately
        });

        return Ok(new { Message = ResponseMessages.Auth.LoggedOutSuccessfully }); 
    }
}