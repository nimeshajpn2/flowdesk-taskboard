using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FlowdeskTaskApi.Test
{
    public class AuthController_RegisterTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly AuthController _controller;

        public AuthController_RegisterTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new AuthController(_userServiceMock.Object);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenSuccessful()
        {
            var model = new RegisterViewModel
            {
                Email = "test@test.com",
                Password = "Password123!"
            };

            // Must return string because RegisterAsync returns Task<string>
            var fakeResult = "user-id-123";

            _userServiceMock
                .Setup(x => x.RegisterAsync(model))
                .ReturnsAsync(fakeResult);

            var result = await _controller.Register(model);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenException()
        {
            var model = new RegisterViewModel
            {
                Email = "test@test.com",
                Password = "Password123!"
            };

            _userServiceMock
                .Setup(x => x.RegisterAsync(model))
                .ThrowsAsync(new System.Exception("Error"));

            var result = await _controller.Register(model);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}