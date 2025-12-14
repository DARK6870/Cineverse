namespace Auth.Authentication;

public static class AuthenticationPolicies
{
    // Policies
    public static readonly string AdminAccessPolicy = nameof(AdminAccessPolicy);
    
    public static readonly string ManagerAccessPolicy = nameof(ManagerAccessPolicy);

    
    // Roles
    public static readonly string[] AdminAccessPolicyRoles = ["Admin"];
    
    public static readonly string[] ManagerAccessPolicyRoles = ["Manager", "Admin"];
}