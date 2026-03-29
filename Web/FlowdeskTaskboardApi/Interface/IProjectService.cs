using FlowdeskTaskboardApi.Models;
using FlowdeskTaskboardApi.Models.ViewModels.Projects;

namespace FlowdeskTaskboardApi.Interface
{
    public interface IProjectService
    {
        Task<Project> CreateAsync(CreateProjectViewModel dto);
        Task UpdateAsync(int id, UpdateProjectViewModel dto);
        Task ArchiveAsync(int id);
        Task<List<Project>> GetAllAsync(bool includeArchived = false);
        Task<Project?> GetByIdAsync(int id);
    }
}
