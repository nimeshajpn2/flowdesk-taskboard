using FlowdeskTaskboardApi.Controllers;
using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels.Projects;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowdeskTaskboardApi.Models;

namespace FlowdeskTaskApi.Test
{
    public class ProjectsControllerTests
    {
        private readonly Mock<IProjectService> _serviceMock;
        private readonly ProjectsController _controller;

        public ProjectsControllerTests()
        {
            _serviceMock = new Mock<IProjectService>();
            _controller = new ProjectsController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAllProjects_ReturnsOk()
        {
            var fakeProjects = new List<Project>
            {
                new Project(),
                new Project()
            };

            _serviceMock.Setup(x => x.GetAllAsync(false))
                .ReturnsAsync(fakeProjects);

            var result = await _controller.GetAllProjects();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetProjectById_ReturnsOk_WhenFound()
        {
            var fakeProject = new Project();

            _serviceMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(fakeProject);

            var result = await _controller.GetProjectById(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetProjectById_ReturnsNotFound_WhenNull()
        {
            _serviceMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Project?)null);

            var result = await _controller.GetProjectById(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateProject_ReturnsOk()
        {
            var fakeProject = new Project();

            _serviceMock.Setup(x => x.CreateAsync(It.IsAny<CreateProjectViewModel>()))
                .ReturnsAsync(fakeProject);

            var result = await _controller.CreateProject(new CreateProjectViewModel());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProject_ReturnsOk()
        {
            _serviceMock.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateProjectViewModel>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateProject(1, new UpdateProjectViewModel());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ArchiveProject_ReturnsOk()
        {
            _serviceMock.Setup(x => x.ArchiveAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.ArchiveProject(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllProjects_ReturnsBadRequest_OnException()
        {
            _serviceMock.Setup(x => x.GetAllAsync(It.IsAny<bool>()))
                .ThrowsAsync(new Exception("error"));

            var result = await _controller.GetAllProjects();

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}