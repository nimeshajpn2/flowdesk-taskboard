namespace FlowdeskTaskboardApi.Shared
{
    public static class TaskStatus
    {
        public const string ToDo = "To Do";
        public const string InProgress = "In Progress";
        public const string Done = "Done";

        // Optional: For validation
        public static readonly List<string> AllStatuses = new()
        {
            ToDo,
            InProgress,
            Done
        };
    }
}
