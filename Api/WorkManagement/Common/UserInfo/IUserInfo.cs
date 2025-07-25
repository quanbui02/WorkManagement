namespace WorkManagement.Common
{
    public interface IUserInfo
    {
        bool IsAuthenticated { get; }

        int UserId { get; }
        string UserIdGuid { get; }

        int IdClient { get; }

        int IdShop { get; }

        int IdPortal { get; }

        string UserName { get; }

        string DisplayName { get; }

        string Avatar { get; }

        string PhoneNumber { get; }

        string Email { get; }

        string Token { get; }

        string FullName { get; }

        bool IsSuperUser { get; }

        string RoleAssign { get; }

        int IdType { get; }

        List<int> ListOrganization { get; }

        List<string> ListRoleAssign { get; }

        bool? IsLeader { get; }
    }
}
