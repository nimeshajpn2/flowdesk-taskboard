namespace FlowdeskTaskboardApi.Shared
{
    //User Roles
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string TeamLead = "TeamLead";
        public const string User = "User";

        public static readonly string[] AllRoles =
        {
            Admin,
            TeamLead,
            User
        };
    }
}
