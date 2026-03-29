using FlowdeskTaskboardApi.CommonData;
using FlowdeskTaskboardApi.Helper;
using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models;
using FlowdeskTaskboardApi.Models.ViewModels.Projects;
using FlowdeskTaskboardApi.Shared;
using Microsoft.Extensions.Logging;

namespace FlowdeskTaskboardApi.Services.ProjectServices
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _logService;
        private readonly IErrorService _errorService;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(
            IUnitOfWork unitOfWork,
            ILogService logService,
            IErrorService errorService,
            ILogger<ProjectService> logger)
        {
            _unitOfWork = unitOfWork;
            _logService = logService;
            _errorService = errorService;
            _logger = logger;
        }

        //Create Project
        public async Task<Project> CreateAsync(CreateProjectViewModel dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new Exception(ErrorMessages.Project.NameRequired);

                var project = new Project
                {
                    Name = dto.Name,
                    Description = dto.Description
                };

                await _unitOfWork.Repository<Project>().AddAsync(project);
                await _unitOfWork.CommitAsync();

                await _logService.LogAsync(
                    CommonConstants.LogLevelInformation,
                    string.Format(LogMessages.Project.Created, project.Id, project.Name),
                    CommonConstants.Create);

                _logger.LogInformation(LogMessages.Project.Created, project.Id, project.Name);

                return project;
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, CommonConstants.Create);
                _logger.LogError(ex, LogMessages.ErrorLogs.ProjectCreateError, dto.Name);
                throw;
            }
        }

        //Update Project
        public async Task UpdateAsync(int id, UpdateProjectViewModel dto)
        {
            try
            {
                var repo = _unitOfWork.Repository<Project>();
                var project = await repo.GetByIdAsync(id);

                if (project == null)
                    throw new Exception(ErrorMessages.Project.NotFound);

                project.Name = dto.Name ?? project.Name;
                project.Description = dto.Description ?? project.Description;

                repo.Update(project);
                await _unitOfWork.CommitAsync();

                await _logService.LogAsync(
                    CommonConstants.LogLevelInformation,
                    string.Format(LogMessages.Project.Updated, project.Id),
                    CommonConstants.Update);

                _logger.LogInformation(LogMessages.Project.Updated, project.Id);
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, CommonConstants.Update);
                _logger.LogError(ex, LogMessages.ErrorLogs.ProjectUpdateError, id);
                throw;
            }
        }

        //Soft Delete
        public async Task ArchiveAsync(int id)
        {
            try
            {
                var repo = _unitOfWork.Repository<Project>();
                var project = await repo.GetByIdAsync(id);

                if (project == null)
                    throw new Exception(ErrorMessages.Project.NotFound);

                project.IsArchived = true;

                repo.Update(project);
                await _unitOfWork.CommitAsync();

                await _logService.LogAsync(
                    CommonConstants.LogLevelInformation,
                    string.Format(LogMessages.Project.Archived, project.Id),
                    CommonConstants.Archive);

                _logger.LogInformation(LogMessages.Project.Archived, project.Id);
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, CommonConstants.Archive);
                _logger.LogError(ex, LogMessages.ErrorLogs.ProjectArchiveError, id);
                throw;
            }
        }

        //Get All Projects
        public async Task<List<Project>> GetAllAsync(bool includeArchived = false)
        {
            var repo = _unitOfWork.Repository<Project>();
            return await repo.FindAllAsync(p => includeArchived || !p.IsArchived);
        }

        //Get By Id
        public async Task<Project?> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<Project>();
            return await repo.GetByIdAsync(id);
        }
    }
}