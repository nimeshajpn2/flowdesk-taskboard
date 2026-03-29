namespace FlowdeskTaskboardApi.Shared
{
    public static class LogMessages
    {
        public static class Auth
        {
            public const string UserRegistered = "User registered: {0}, Role: {1}";
            public const string UserLoggedIn = "User logged in: {0}";
        }

        public static class Project
        {
            public const string Created = "Project created: {0} - {1}";
            public const string Updated = "Project updated: {0}";
            public const string Archived = "Project archived: {0}";
        }

        public static class Task
        {
            public const string Created = "Task created: {0} - {1}";
            public const string Updated = "Task updated: {0}";
            public const string Archived = "Task archived: {0}";
            public const string StatusUpdated = "Task status updated: {0} -> {1}";
            public const string TasksFetched = "Fetched tasks for project: {0}";
            public const string TasksFetchedCount = "Fetched {0} tasks for project {1}";
        }

        public static class ErrorLogs
        {
            public const string RegisterError = "Error registering user {0}";
            public const string LoginError = "Error logging in user {0}";
            public const string ProjectCreateError = "Error creating project {0}";
            public const string ProjectUpdateError = "Error updating project {0}";
            public const string ProjectArchiveError = "Error archiving project {0}";
            public const string TaskCreateError = "Error creating task";
            public const string TaskUpdateError = "Error updating task {0}";
            public const string TaskArchiveError = "Error archiving task {0}";
            public const string TaskStatusUpdateError = "Error updating status for task {0}";
            public const string GetTasksError = "Error fetching tasks for project {0}";
        }
    }
}
