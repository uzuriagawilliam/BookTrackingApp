using Microsoft.AspNetCore.Authorization;

namespace BookTrackingApp.Models
{
    public static class AuthorizationPolicies
    {
        public static AuthorizationPolicy CanAddBook()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("country", "se", "co")
                .RequireRole("user", "admin")
                .Build();
        }
    }
}
