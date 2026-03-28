using FlowdeskTaskboardApi.CommonData;
using FlowdeskTaskboardApi.Helper;
using FlowdeskTaskboardApi.Models;
using FlowdeskTaskboardApi.Models.ViewModels;
using Microsoft.Extensions.Logging;

namespace FlowdeskTaskboardApi.Services.TaskServices
{
    public class TaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _logService;
        private readonly IErrorService _errorService;
        private readonly ILogger<TaskService> _logger;

        public TaskService(
            IUnitOfWork unitOfWork,
            ILogService logService,
            IErrorService errorService,
            ILogger<TaskService> logger)
        {
            _unitOfWork = unitOfWork;
            _logService = logService;
            _errorService = errorService;
            _logger = logger;
        }

        //Create task
        public async Task<TaskItem?> CreateAsync(CreateTaskViewModel dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                    throw new Exception("Title cannot be empty");

                if (dto.DueDate < DateTime.UtcNow)
                    throw new Exception("Due date cannot be in the past");

                var task = new TaskItem
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    AssignedToUserId = dto.AssignedToUserId,
                    Priority = dto.Priority,
                    DueDate = dto.DueDate,
                    Status = "ToDo"
                };

                await _unitOfWork.Repository<TaskItem>().AddAsync(task);
                await _unitOfWork.CommitAsync();

                // Log to both DB and ILogger
                await _logService.LogAsync("Information", $"Task created: {task.Title}", source: "CreateAsync");
                _logger.LogInformation("Task created: {TaskId} - {Title}", task.Id, task.Title);

                return task;
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, "CreateAsync");
                _logger.LogError(ex, "Error creating task");
                throw;
            }
        }

        //Update task details 
        public async Task UpdateAsync(int id, UpdateTaskViewModel dto)
        {
            try
            {
                var repo = _unitOfWork.Repository<TaskItem>();
                var task = await repo.GetByIdAsync(id);

                if (task == null)
                    throw new Exception("Task not found");

                if (!string.IsNullOrWhiteSpace(dto.Title))
                    task.Title = dto.Title;

                if (dto.DueDate.HasValue && dto.DueDate < DateTime.UtcNow)
                    throw new Exception("Invalid due date");

                task.Description = dto.Description ?? task.Description;
                task.Priority = dto.Priority ?? task.Priority;
                task.DueDate = dto.DueDate ?? task.DueDate;

                repo.Update(task);
                await _unitOfWork.CommitAsync();

                await _logService.LogAsync("Information", $"Task updated: {task.Id}", source: "UpdateAsync");
                _logger.LogInformation("Task updated: {TaskId}", task.Id);
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, "UpdateAsync");
                _logger.LogError(ex, "Error updating task {TaskId}", id);
                throw;
            }
        }

        //Update status
        public async Task UpdateStatusAsync(int id, string status)
        {
            try
            {
                var repo = _unitOfWork.Repository<TaskItem>();
                var task = await repo.GetByIdAsync(id);

                if (task == null)
                    throw new Exception("Task not found");

                if (task.Status == "Done")
                    throw new Exception("Cannot change completed task");

                task.Status = status;

                repo.Update(task);
                await _unitOfWork.CommitAsync();

                await _logService.LogAsync("Information", $"Task status updated: {task.Id} -> {status}", source: "UpdateStatusAsync");
                _logger.LogInformation("Task status updated: {TaskId} -> {Status}", task.Id, status);
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, "UpdateStatusAsync");
                _logger.LogError(ex, "Error updating status for task {TaskId}", id);
                throw;
            }
        }

        //Archive task
        public async Task ArchiveAsync(int id)
        {
            try
            {
                var repo = _unitOfWork.Repository<TaskItem>();
                var task = await repo.GetByIdAsync(id);

                if (task == null)
                    throw new Exception("Task not found");

                task.IsArchived = true;

                repo.Update(task);
                await _unitOfWork.CommitAsync();

                await _logService.LogAsync("Information", $"Task archived: {task.Id}", source: "ArchiveAsync");
                _logger.LogInformation("Task archived: {TaskId}", task.Id);
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, "ArchiveAsync");
                _logger.LogError(ex, "Error archiving task {TaskId}", id);
                throw;
            }
        }
    }
}