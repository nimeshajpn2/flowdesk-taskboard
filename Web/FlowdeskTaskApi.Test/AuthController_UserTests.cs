using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels.Auth.FlowdeskTaskboardApi.Models.ViewModels.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Language.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FlowdeskTaskApi.Test
{
    public class AuthController_UserTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly AuthController _controller;

        public AuthController_UserTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new AuthController(_userServiceMock.Object);

            // Mock HttpContext for cookies and user
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async Task Me_ReturnsUnauthorized_WhenNoClaim()
        {
            // No user claim
            var result = await _controller.Me();

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Me_ReturnsNotFound_WhenUserNotExists()
        {
            var userId = "123";

            // Set user claim
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };
            _controller.ControllerContext.HttpContext.User =
                new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Mock service to return null
            _userServiceMock
             .Setup(x => x.GetByIdAsync(userId))
             .ReturnsAsync((UserViewModel?)null); 
            var result = await _controller.Me();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Me_ReturnsOk_WhenUserExists()
        {
            var userId = "123";

            var fakeUser = new UserViewModel
            {
                Id = userId,
                Email = "test@test.com"
            };

            // Set user claim
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };
            _controller.ControllerContext.HttpContext.User =
                new ClaimsPrincipal(new ClaimsIdentity(claims));

            // Mock service to return a valid user
            _userServiceMock
                .Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(fakeUser);

            var result = await _controller.Me();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void Logout_ReturnsOk_AndClearsCookie()
        {
            var result = _controller.Logout();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            // Ensure cookie header is set (cleared)
            Assert.True(_controller.Response.Headers.ContainsKey("Set-Cookie"));
        }
    }

}
