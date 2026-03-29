using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowdeskTaskApi.Test
{
    public class AuthController_LoginTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly AuthController _controller;

        public AuthController_LoginTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new AuthController(_userServiceMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async Task Login_ReturnsOk_AndSetsCookie()
        {
            var model = new LoginViewModel();
            var token = "fake-token";

            _userServiceMock.Setup(x => x.LoginAsync(model))
                            .ReturnsAsync(token);

            var result = await _controller.Login(model);

            Assert.IsType<OkObjectResult>(result);
            Assert.True(_controller.Response.Headers.ContainsKey("Set-Cookie"));
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenException()
        {
            var model = new LoginViewModel();

            _userServiceMock.Setup(x => x.LoginAsync(model))
                            .ThrowsAsync(new System.Exception("Invalid"));

            var result = await _controller.Login(model);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
