using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels;
using FlowdeskTaskboardApi.Models.ViewModels.Auth;
using FlowdeskTaskboardApi.Models.ViewModels.Auth.FlowdeskTaskboardApi.Models.ViewModels.Auth;
using FlowdeskTaskboardApi.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowdeskTaskboardApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,TeamLead")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsersAsync();

                return Ok(new ApiResponse<List<UserViewModel>>
                {
                    Message = "Users fetched successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //Get User By Id
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(id);

                if (result == null)
                    return NotFound(new { Message = "User not found" });

                return Ok(new ApiResponse<UserViewModel>
                {
                    Message = "User fetched successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
