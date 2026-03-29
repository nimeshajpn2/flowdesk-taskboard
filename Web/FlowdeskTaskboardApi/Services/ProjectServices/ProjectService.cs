using FlowdeskTaskboardApi.CommonData;
using FlowdeskTaskboardApi.Helper;
using FlowdeskTaskboardApi.Interface;
using FlowdeskTaskboardApi.Models;
using FlowdeskTaskboardApi.Models.ViewModels.Projects;
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

        // Create Project
        public async Task<Project> CreateAsync(CreateProjectViewModel dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new Exception("Project name cannot be empty");

                var project = new Project
                {
                    Name = dto.Name,
                    Description = dto.Description
                };

                await _unitOfWork.Repository<Project>().AddAsync(project);
                await _unitOfWork.CommitAsync();

                // Logging
                await _logService.LogAsync("Information", $"Project created: {project.Name}", source: "CreateAsync");
                _logger.LogInformation("Project created: {ProjectId} - {Name}", project.Id, project.Name);

                return project;
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, "CreateAsync");
                _logger.LogError(ex, "Error creating project {Name}", dto.Name);
                throw;
            }
        }

        // Update Project
        public async Task UpdateAsync(int id, UpdateProjectViewModel dto)
        {
            try
            {
                var repo = _unitOfWork.Repository<Project>();
                var project = await repo.GetByIdAsync(id);

                if (project == null)
                    throw new Exception("Project not found");

                project.Name = dto.Name ?? project.Name;
                project.Description = dto.Description ?? project.Description;

                repo.Update(project);
                await _unitOfWork.CommitAsync();

                // Logging
                await _logService.LogAsync("Information", $"Project updated: {project.Id}", source: "UpdateAsync");
                _logger.LogInformation("Project updated: {ProjectId}", project.Id);
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, "UpdateAsync");
                _logger.LogError(ex, "Error updating project {ProjectId}", id);
                throw;
            }
        }

        // Archive Project
        public async Task ArchiveAsync(int id)
        {
            try
            {
                var repo = _unitOfWork.Repository<Project>();
                var project = await repo.GetByIdAsync(id);

                if (project == null)
                    throw new Exception("Project not found");

                project.IsArchived = true;

                repo.Update(project);
                await _unitOfWork.CommitAsync();

                // Logging
                await _logService.LogAsync("Information", $"Project archived: {project.Id}", source: "ArchiveAsync");
                _logger.LogInformation("Project archived: {ProjectId}", project.Id);
            }
            catch (Exception ex)
            {
                await _errorService.SaveErrorAsync(ex, "ArchiveAsync");
                _logger.LogError(ex, "Error archiving project {ProjectId}", id);
                throw;
            }
        }

        // Get All Projects
        public async Task<List<Project>> GetAllAsync(bool includeArchived = false)
        {
            var repo = _unitOfWork.Repository<Project>();
            var projects = await repo.FindAllAsync(p => includeArchived || !p.IsArchived);
            return projects;
        }

        // Get Project By Id
        public async Task<Project?> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<Project>();
            return await repo.GetByIdAsync(id);
        }
    }
}