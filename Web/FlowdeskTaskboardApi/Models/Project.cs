namespace FlowdeskTaskboardApi.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsArchived { get; set; } = false;
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
