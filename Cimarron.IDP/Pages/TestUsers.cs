// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using IdentityModel;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;

namespace Cimarron.IDP;

public class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
            var address = new
            {
                street_address = "Rågsvedsvägen 70",
                locality = "Bandhagen",
                postal_code = 12465,
                country = "Sweden"
            };
                
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "William",
                    Password = "William",
                    Claims =
                    {
                        //new Claim(JwtClaimTypes.Role, "Admin"),
                        new Claim("role", "admin"),
                        new Claim("country", "co"),
                        new Claim(JwtClaimTypes.Id, "1"),
                        new Claim(JwtClaimTypes.Name, "William Uzuriaga"),
                        new Claim(JwtClaimTypes.GivenName, "William"),
                        new Claim(JwtClaimTypes.FamilyName, "Uzuriaga"),
                        new Claim(JwtClaimTypes.Email, "WilliamUzuriaga@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "bob",
                    Claims =
                    {
                        //new Claim(JwtClaimTypes.Role, "User"),
                        new Claim("role", "user"),
                        new Claim("country", "se"),
                        new Claim(JwtClaimTypes.Id, "2"),
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                    }
                },
                new TestUser
                {
                    SubjectId = "3",
                    Username = "Kalle",
                    Password = "Kalle",
                    Claims =
                    {
                        //new Claim(JwtClaimTypes.Role, "User"),
                        new Claim("role", "gest"),
                        new Claim("country", "usa"),
                        new Claim(JwtClaimTypes.Id, "3"),
                        new Claim(JwtClaimTypes.Name, "Kalle Anka"),
                        new Claim(JwtClaimTypes.GivenName, "Kalle"),
                        new Claim(JwtClaimTypes.FamilyName, "Anka"),
                        new Claim(JwtClaimTypes.Email, "kalleanka@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://anka.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
            };
        }
    }
}