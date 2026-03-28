using FlowdeskTaskboardApi.Models.ViewModels;
using FlowdeskTaskboardApi.Services.TaskServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowdeskTaskboardApi.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _service;

        public TasksController(TaskService service)
        {
            _service = service;
        }

        // Create Task
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskViewModel dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        //Update Task
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTaskViewModel dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        //Update Status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateStatusViewModel dto)
        {
            await _service.UpdateStatusAsync(id, dto.Status);
            return NoContent();
        }

        //Archive
        [HttpDelete("{id}")]
        public async Task<IActionResult> Archive(int id)
        {
            await _service.ArchiveAsync(id);
            return NoContent();
        }
    }
}
