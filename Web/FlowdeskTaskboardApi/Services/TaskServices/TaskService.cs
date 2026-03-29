using FlowdeskTaskboardApi.CommonData;
using FlowdeskTaskboardApi.Helper;
using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models;
using FlowdeskTaskboardApi.Models.ViewModels;
using FlowdeskTaskboardApi.Models.ViewModels.Task;
using FlowdeskTaskboardApi.Shared;
using Microsoft.Extensions.Logging;

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
                    throw new Exception(ErrorMessages.Task.TitleRequired);

                if (dto.DueDate < DateTime.UtcNow)
                    throw new Exception(ErrorMessages.Task.InvalidDueDate);

                var project = await _unitOfWork.Repository<Project>().GetByIdAsync(dto.ProjectId);
                if (project == null)
                    throw new Exception(ErrorMessages.Project.NotFound);

                var task = new TaskItem
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    AssignedToUserId = dto.AssignedToUserId,
                    Priority = dto.Priority,
                    DueDate = dto.DueDate,
                    Status = Shared.TaskStatus.ToDo,
                    ProjectId = dto.ProjectId
                };

                await _unitOfWork.Repository<TaskItem>().AddAsync(task);
                await _unitOfWork.CommitAsync();

                await _logService.LogAsync(
                    CommonConstants.LogLevelInformation,
                    string.Format(LogMessages.Task.Created, task.Id, task.Title),
                    CommonConstants.Create);

                _logger.LogInformation(LogMessages.Task.Created, task.Id, task.Title);

                return new ResponseTaskItemViewModel
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
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, CommonConstants.Create);
                _logger.LogError(ex, LogMessages.ErrorLogs.TaskCreateError);
                throw;
            }
        }

        //Update Task
        public async Task UpdateAsync(int id, UpdateTaskViewModel dto)
        {
            try
            {
                var repo = _unitOfWork.Repository<TaskItem>();
                var task = await repo.GetByIdAsync(id);

                if (task == null)
                    throw new Exception(ErrorMessages.Task.NotFound);

                if (!string.IsNullOrWhiteSpace(dto.Title))
                    task.Title = dto.Title;

                if (dto.DueDate.HasValue && dto.DueDate < DateTime.UtcNow)
                    throw new Exception(ErrorMessages.Task.InvalidDueDate);

                task.Description = dto.Description ?? task.Description;
                task.Priority = dto.Priority ?? task.Priority;
                task.DueDate = dto.DueDate ?? task.DueDate;

                if (dto.ProjectId.HasValue)
                {
                    var project = await _unitOfWork.Repository<Project>().GetByIdAsync(dto.ProjectId.Value);
                    if (project == null)
                        throw new Exception(ErrorMessages.Project.NotFound);

                    task.ProjectId = dto.ProjectId.Value;
                }

                repo.Update(task);
                await _unitOfWork.CommitAsync();

                await _logService.LogAsync(
                    CommonConstants.LogLevelInformation,
                    string.Format(LogMessages.Task.Updated, task.Id),
                    CommonConstants.Update);

                _logger.LogInformation(LogMessages.Task.Updated, task.Id);
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, CommonConstants.Update);
                _logger.LogError(ex, LogMessages.ErrorLogs.TaskUpdateError, id);
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
                    throw new Exception(ErrorMessages.Task.NotFound);

                // Check if task is already completed
                if (task.Status == Shared.TaskStatus.Done)
                    throw new Exception(ErrorMessages.Task.AlreadyCompleted);

                // Validate new status
                if (!Shared.TaskStatus.AllStatuses.Contains(status))
                    throw new Exception(ErrorMessages.Task.InvalidStatus);

                task.Status = status;

                repo.Update(task);
                await _unitOfWork.CommitAsync();

                // Logging
                await _logService.LogAsync(
                    CommonConstants.LogLevelInformation,
                    string.Format(LogMessages.Task.StatusUpdated, task.Id, status),
                    CommonConstants.UpdateStatus);

                _logger.LogInformation(LogMessages.Task.StatusUpdated, task.Id, status);
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, CommonConstants.UpdateStatus);
                _logger.LogError(ex, LogMessages.ErrorLogs.TaskStatusUpdateError, id);
                throw;
            }
        }

        //Soft Delete
        public async Task ArchiveAsync(int id)
        {
            try
            {
                var repo = _unitOfWork.Repository<TaskItem>();
                var task = await repo.GetByIdAsync(id);

                if (task == null)
                    throw new Exception(ErrorMessages.Task.NotFound);

                task.IsArchived = true;

                repo.Update(task);
                await _unitOfWork.CommitAsync();

                await _logService.LogAsync(
                    CommonConstants.LogLevelInformation,
                    string.Format(LogMessages.Task.Archived, task.Id),
                    CommonConstants.Archive);

                _logger.LogInformation(LogMessages.Task.Archived, task.Id);
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, CommonConstants.Archive);
                _logger.LogError(ex, LogMessages.ErrorLogs.TaskArchiveError, id);
                throw;
            }
        }

        //Get Tasks by Project 
        public async Task<List<TaskItem>> GetTasksByProjectAsync(
        int projectId,
        string? status = null,
        string? priority = null,
        int? assignedToUserId = null,
        int pageNumber = 1,
        int pageSize = 10)
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

                // Pagination
                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var tasks = query.ToList();

                await _logService.LogAsync(
                    CommonConstants.LogLevelInformation,
                    string.Format(LogMessages.Task.TasksFetched, projectId),
                    CommonConstants.GetTasks);

                _logger.LogInformation(
                    LogMessages.Task.TasksFetchedCount,
                    tasks.Count,
                    projectId);

                return await Task.FromResult(tasks);
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, CommonConstants.GetTasks);
                _logger.LogError(ex, LogMessages.ErrorLogs.GetTasksError, projectId);
                throw;
            }
        }
    }
}