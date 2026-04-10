using FlowdeskTaskboardApi.Controllers;
using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models;
using FlowdeskTaskboardApi.Models.ViewModels;
using FlowdeskTaskboardApi.Models.ViewModels.Task;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FlowdeskTaskApi.Test
{
    public class TasksControllerTests
    {
        private readonly Mock<ITaskService> _serviceMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _serviceMock = new Mock<ITaskService>();
            _controller = new TasksController(_serviceMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsOk()
        {
            var fakeResult = new ResponseTaskItemViewModel
            {

            };

            _serviceMock
                .Setup(x => x.CreateAsync(It.IsAny<CreateTaskViewModel>()))
                .ReturnsAsync(fakeResult);

            var result = await _controller.Create(new CreateTaskViewModel());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsOk()
        {
            _serviceMock.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateTaskViewModel>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.Update(1, new UpdateTaskViewModel());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateStatus_ReturnsOk()
        {
            _serviceMock.Setup(x => x.UpdateStatusAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateStatus(1, new UpdateStatusViewModel
            {
                Status = "Done"
            });

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Archive_ReturnsOk()
        {
            _serviceMock.Setup(x => x.ArchiveAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.Archive(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetTasksByProject_ReturnsOk()
        {
            var fakeTasks = new List<TaskItem>
            {
                new TaskItem(),
                new TaskItem()
            };

                    _serviceMock.Setup(x =>
                        x.GetTasksByProjectAsync(
                            It.IsAny<int>(),
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<int?>(),
                            It.IsAny<int>(),
                            It.IsAny<int>()
                        ))
                .ReturnsAsync(fakeTasks);

            var result = await _controller.GetTasksByProject(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_OnException()
        {
            _serviceMock.Setup(x => x.CreateAsync(It.IsAny<CreateTaskViewModel>()))
                .ThrowsAsync(new Exception("error"));

            var result = await _controller.Create(new CreateTaskViewModel());

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}