using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels;
using FlowdeskTaskboardApi.Models.ViewModels.Projects;
using FlowdeskTaskboardApi.Services.ProjectServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowdeskTaskboardApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/projects")]

    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
        {
            _service = service;
        }

        //Get All Projects
        [HttpGet]
        public async Task<IActionResult> GetAllProjects(bool includeArchived = false)
        {
            var result = await _service.GetAllAsync(includeArchived);
            return Ok(result);
        }

        //Get Project By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _service.GetByIdAsync(id);
            if (project == null)
                return NotFound();
            return Ok(project);
        }

        //Create Project
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectViewModel dto)
        {
            var project = await _service.CreateAsync(dto);
            return Ok(project);
        }

        //Update Project
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectViewModel dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        //Archive Project
        [HttpDelete("{id}")]
        public async Task<IActionResult> ArchiveProject(int id)
        {
            await _service.ArchiveAsync(id);
            return NoContent();
        }
    }
}