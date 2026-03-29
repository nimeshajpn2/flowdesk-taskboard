namespace FlowdeskTaskboardApi.Models.ViewModels.Task
{
    public class ResponseTaskItemViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public int AssignedToUserId { get; set; }

        public string? Priority { get; set; } 

        public DateTime DueDate { get; set; }

        public string? Status { get; set; } 

        public bool? IsArchived { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int ProjectId { get; set; }
    }
}
