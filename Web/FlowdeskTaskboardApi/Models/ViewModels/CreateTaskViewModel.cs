namespace FlowdeskTaskboardApi.Models.ViewModels
{
    public class CreateTaskViewModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int AssignedToUserId { get; set; }
        public string? Priority { get; set; }
        public DateTime DueDate { get; set; }
    }
}
