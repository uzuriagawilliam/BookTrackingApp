using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Cimarron.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", "Your role(s)", new[]{"role"}),
            new IdentityResource("country", "The country you are from", new List<string>(){"country"})
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            { };

    public static IEnumerable<Client> Clients =>
        new Client[] 
            { 
                new Client()
                {
                    ClientName = "Book Tracking App",
                    ClientId = "booktrackingapp",
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris =
                    {
                        "https://localhost:7035/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:7035/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "country"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RequireConsent = true
                }
            };
}