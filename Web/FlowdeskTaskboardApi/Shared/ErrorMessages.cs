namespace FlowdeskTaskboardApi.Shared
{
    public static class ErrorMessages
    {
        public static class Auth
        {
            public const string InvalidRole = "Invalid role";
            public const string InvalidUser = "Invalid User";
            public const string InvalidPassword = "Invalid Password";
        }

        public static class Project
        {
            public const string NotFound = "Project not found";
            public const string NameRequired = "Project name cannot be empty";
        }

        public static class Task
        {
            public const string NotFound = "Task not found";
            public const string TitleRequired = "Title cannot be empty";
            public const string InvalidDueDate = "Due date cannot be in the past";
            public const string AlreadyCompleted = "Cannot change completed task";
            public const string InvalidStatus = "Invalid Status";
        }
    }
}
