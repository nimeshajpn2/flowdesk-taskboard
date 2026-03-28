namespace FlowdeskTaskboardApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public int AssignedToUserId { get; set; }

        public string? Priority { get; set; } // Low, Medium, High

        public DateTime DueDate { get; set; }

        public string? Status { get; set; } // ToDo, InProgress, Done

        public bool IsArchived { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key to Project
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}
