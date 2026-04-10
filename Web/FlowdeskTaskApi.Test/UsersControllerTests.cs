using FlowdeskTaskboardApi.Controllers;
using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels.Auth;
using FlowdeskTaskboardApi.Models.ViewModels.Auth.FlowdeskTaskboardApi.Models.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FlowdeskTaskApi.Test
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UsersController(_userServiceMock.Object);
        }

        // GET ALL USERS - SUCCESS
        [Fact]
        public async Task GetAllUsers_ReturnsOk_WhenUsersExist()
        {
            var fakeUsers = new List<UserViewModel>
            {
                new UserViewModel
                {
                    Id = "1",
                    UserName = "user1",
                    Email = "user1@test.com"
                },
                new UserViewModel
                {
                    Id = "2",
                    UserName = "user2",
                    Email = "user2@test.com"
                }
            };

            _userServiceMock
                .Setup(x => x.GetAllUsersAsync())
                .ReturnsAsync(fakeUsers);

            var result = await _controller.GetAllUsers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        // GET USER BY ID - SUCCESS
        [Fact]
        public async Task GetUserById_ReturnsOk_WhenUserExists()
        {
            var userId = "123";

            var fakeUser = new UserViewModel
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@test.com"
            };

            _userServiceMock
                .Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync(fakeUser);

            var result = await _controller.GetUserById(userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        // GET USER BY ID - NOT FOUND
        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenUserNull()
        {
            var userId = "999";

            _userServiceMock
                .Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync((UserViewModel?)null);

            var result = await _controller.GetUserById(userId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // GET ALL USERS - EXCEPTION
        [Fact]
        public async Task GetAllUsers_ReturnsBadRequest_WhenExceptionThrown()
        {
            _userServiceMock
                .Setup(x => x.GetAllUsersAsync())
                .ThrowsAsync(new Exception("Database error"));

            var result = await _controller.GetAllUsers();

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}