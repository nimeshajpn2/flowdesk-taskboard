using FlowdeskTaskboardApi.Models;
using FlowdeskTaskboardApi.Models.ViewModels;
using FlowdeskTaskboardApi.Models.ViewModels.Task;
using System.Threading.Tasks;

namespace FlowdeskTaskboardApi.Interface
{
    public interface ITaskService
    {
        Task<ResponseTaskItemViewModel?> CreateAsync(CreateTaskViewModel dto);
        Task UpdateAsync(int id, UpdateTaskViewModel dto);
        Task UpdateStatusAsync(int id, string status);
        Task ArchiveAsync(int id);
        Task<List<TaskItem>> GetTasksByProjectAsync(int projectId, string? status = null, string? priority = null, int? assignedToUserId = null, int pageNumber = 1, int pageSize = 10);
    }
}
