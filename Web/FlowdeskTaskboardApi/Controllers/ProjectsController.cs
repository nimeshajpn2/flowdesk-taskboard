using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels.Projects;
using FlowdeskTaskboardApi.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowdeskTaskboardApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,TeamLead")]
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
            try
            {
                var result = await _service.GetAllAsync(includeArchived);
                return Ok(new { Message = ResponseMessages.Project.FetchAllSuccess, Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //Get Project By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            try
            {
                var project = await _service.GetByIdAsync(id);
                if (project == null)
                    return NotFound(new { Message = ResponseMessages.Project.NotFound });

                return Ok(new { Message = ResponseMessages.Project.FetchSuccess, Data = project });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //Create Project
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectViewModel dto)
        {
            try
            {
                var project = await _service.CreateAsync(dto);
                return Ok(new { Message = ResponseMessages.Project.CreateSuccess, Data = project });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //Update Project
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectViewModel dto)
        {
            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok(new { Message = ResponseMessages.Project.UpdateSuccess });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //Archive Project
        [HttpDelete("{id}")]
        public async Task<IActionResult> ArchiveProject(int id)
        {
            try
            {
                await _service.ArchiveAsync(id);
                return Ok(new { Message = ResponseMessages.Project.ArchiveSuccess });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}