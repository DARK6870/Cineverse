using Auth.Models.Enums;

namespace Auth.Authentication;

public static class AuthenticationPolicies
{
    // Policies
    public static readonly string AdminAccessPolicy = nameof(AdminAccessPolicy);
    
    public static readonly string ManagerAccessPolicy = nameof(ManagerAccessPolicy);

    
    // Roles
    public static readonly string[] AdminAccessPolicyRoles = [nameof(Role.Admin)];
    
    public static readonly string[] ManagerAccessPolicyRoles = [nameof(Role.Admin), nameof(Role.Manager)];
}