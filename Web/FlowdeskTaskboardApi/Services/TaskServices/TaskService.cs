using FlowdeskTaskboardApi.CommonData;
using FlowdeskTaskboardApi.Helper;
using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models;
using FlowdeskTaskboardApi.Models.ViewModels;
using FlowdeskTaskboardApi.Models.ViewModels.Task;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Threading.Tasks;

namespace FlowdeskTaskboardApi.Services.TaskServices
{
    public class TaskService : ITaskService
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

        //Create Task
        public async Task<ResponseTaskItemViewModel?> CreateAsync(CreateTaskViewModel dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                    throw new Exception("Title cannot be empty");

                if (dto.DueDate < DateTime.UtcNow)
                    throw new Exception("Due date cannot be in the past");

                // Check project exists
                var project = await _unitOfWork.Repository<Project>().GetByIdAsync(dto.ProjectId);
                if (project == null)
                    throw new Exception("Project not found");

                var task = new TaskItem
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    AssignedToUserId = dto.AssignedToUserId,
                    Priority = dto.Priority,
                    DueDate = dto.DueDate,
                    Status = "ToDo",
                    ProjectId = dto.ProjectId
                };

                await _unitOfWork.Repository<TaskItem>().AddAsync(task);
                await _unitOfWork.CommitAsync();

                await _logService.LogAsync("Information", $"Task created: {task.Title}", source: "CreateAsync");
                _logger.LogInformation("Task created: {TaskId} - {Title}", task.Id, task.Title);

                //Create Response Object
                var ResponseTaskItem = new ResponseTaskItemViewModel
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    AssignedToUserId = task.AssignedToUserId,
                    Priority = task.Priority,
                    DueDate = task.DueDate,
                    Status = task.Status,
                    IsArchived = task.IsArchived,
                    CreatedAt = task.CreatedAt,
                    ProjectId = task.ProjectId
                };

                return ResponseTaskItem;
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, "CreateAsync");
                _logger.LogError(ex, "Error creating task");
                throw;
            }
        }

        //Update Task Details
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

                // Update Project if provided
                if (dto.ProjectId.HasValue)
                {
                    var project = await _unitOfWork.Repository<Project>().GetByIdAsync(dto.ProjectId.Value);
                    if (project == null)
                        throw new Exception("Project not found");

                    task.ProjectId = dto.ProjectId.Value;
                }

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

        //Update Status
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

        //Archive Task
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

        //Get Tasks By Project 
        public async Task<List<TaskItem>> GetTasksByProjectAsync(int projectId, string? status = null, string? priority = null, int? assignedToUserId = null)
        {
            try
            {
                var repo = _unitOfWork.Repository<TaskItem>();
                var query = repo.Query().Where(t => t.ProjectId == projectId && !t.IsArchived);

                if (!string.IsNullOrWhiteSpace(status))
                    query = query.Where(t => t.Status == status);

                if (!string.IsNullOrWhiteSpace(priority))
                    query = query.Where(t => t.Priority == priority);

                if (assignedToUserId.HasValue)
                    query = query.Where(t => t.AssignedToUserId == assignedToUserId.Value);

                var tasks = await Task.FromResult(query.ToList());

                await _logService.LogAsync("Information", $"Fetched tasks for project: {projectId}", source: "GetTasksByProjectAsync");
                _logger.LogInformation("Fetched {Count} tasks for project {ProjectId}", tasks.Count, projectId);

                return tasks;
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, "GetTasksByProjectAsync");
                _logger.LogError(ex, "Error fetching tasks for project {ProjectId}", projectId);
                throw;
            }
        }
    }
}