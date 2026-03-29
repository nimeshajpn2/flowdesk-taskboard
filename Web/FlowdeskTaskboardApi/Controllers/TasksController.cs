using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models.ViewModels;
using FlowdeskTaskboardApi.Models.ViewModels.Task;
using FlowdeskTaskboardApi.Shared;
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
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(new { Message = ResponseMessages.Task.CreateSuccess, Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Update Task Details
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskViewModel dto)
        {
            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok(new { Message = ResponseMessages.Task.UpdateSuccess });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Update Status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusViewModel dto)
        {
            try
            {
                await _service.UpdateStatusAsync(id, dto.Status);
                return Ok(new { Message = ResponseMessages.Task.StatusUpdated });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Archive Task
        [HttpDelete("{id}")]
        public async Task<IActionResult> Archive(int id)
        {
            try
            {
                await _service.ArchiveAsync(id);
                return Ok(new { Message = ResponseMessages.Task.ArchiveSuccess });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Get Tasks By Project 
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetTasksByProject(
            int projectId,
            [FromQuery] string? status = null,
            [FromQuery] string? priority = null,
            [FromQuery] int? assignedToUserId = null)
        {
            try
            {
                var tasks = await _service.GetTasksByProjectAsync(projectId, status, priority, assignedToUserId);
                return Ok(new { Message = ResponseMessages.Task.FetchByProjectSuccess, Data = tasks });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}