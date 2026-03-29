namespace FlowdeskTaskboardApi.Models.ViewModels
{
    public class UpdateTaskViewModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int? ProjectId { get; set; }
    }
}
