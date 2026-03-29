using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels;
using FlowdeskTaskboardApi.Services.TaskServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlowdeskTaskboardApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/tasks")]
    
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service)
        {
            _service = service;
        }

        // Create Task
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskViewModel dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        // Update Task Details
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskViewModel dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        // Update Status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusViewModel dto)
        {
            await _service.UpdateStatusAsync(id, dto.Status);
            return NoContent();
        }

        // Archive Task
        [HttpDelete("{id}")]
        public async Task<IActionResult> Archive(int id)
        {
            await _service.ArchiveAsync(id);
            return NoContent();
        }

        // Get Tasks By Project 
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetTasksByProject(
            int projectId,
            [FromQuery] string? status = null,
            [FromQuery] string? priority = null,
            [FromQuery] int? assignedToUserId = null)
        {
            var tasks = await _service.GetTasksByProjectAsync(projectId, status, priority, assignedToUserId);
            return Ok(tasks);
        }
    }
}