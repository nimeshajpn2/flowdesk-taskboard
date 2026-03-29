namespace FlowdeskTaskboardApi.Shared
{
    public static class ResponseMessages
    {
        public static class Auth
        {
            public const string UserRegistered = "User registered successfully";
            public const string LoginSuccess = "Login successful";
            public const string RegisterSuccess = "User registered successfully";  
            public const string InvalidCredentials = "Invalid username or password";
        }

        public static class Project
        {
            public const string Created = "Project created successfully";
            public const string Updated = "Project updated successfully";
            public const string Archived = "Project archived successfully";
            public const string FetchSuccess = "Project fetched successfully";
            public const string NotFound = "Project not found";
            public const string InvalidName = "Project name cannot be empty";
            public const string CreateSuccess = "Project created successfully";
            public const string UpdateSuccess = "Project updated successfully";
            public const string ArchiveSuccess = "Project archived successfully";
            public const string FetchAllSuccess = "Projects fetched successfully"; 
        }

        public static class Task
        {
            public const string Created = "Task created successfully";
            public const string Updated = "Task updated successfully";
            public const string Archived = "Task archived successfully";
            public const string StatusUpdated = "Task status updated successfully";
            public const string FetchByProjectSuccess = "Tasks fetched successfully by project";
            public const string NotFound = "Task not found";
            public const string InvalidTitle = "Title cannot be empty";
            public const string InvalidDueDate = "Due date cannot be in the past";
            public const string AlreadyCompleted = "Cannot change a completed task";
            public const string InvalidStatus = "Invalid task status. Allowed values: ToDo, InProgress, Done";
            public const string CreateSuccess = "Task creation successful";
            public const string UpdateSuccess = "Task update successful";
            public const string ArchiveSuccess = "Task archive successful";
        }
    }
}
