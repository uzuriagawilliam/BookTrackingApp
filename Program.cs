
using BookTrackingApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("BookContextConnection")
    ?? throw new InvalidOperationException("Connection string 'BookContextConnection' not found.");

builder.Services.AddDbContext<BookContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddLogging(); 

builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddControllersWithViews();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.AccessDeniedPath = "/Authentication/AccessDenied";
}
)


.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.Authority = "https://localhost:5001";
    options.ClientId = "booktrackingapp";
    options.ClientSecret = "secret";
    options.ResponseType = "code";
    //options.Scope.Add("openid");
    //options.Scope.Add("profile");
    //options.CallbackPath = new PathString("singin-oidc");
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ClaimActions.Remove("aud");
    options.ClaimActions.DeleteClaim("sid");
    options.ClaimActions.DeleteClaim("idp");
    options.Scope.Add("roles");
    options.ClaimActions.MapJsonKey("role", "role");
    options.TokenValidationParameters = new()
    {
        NameClaimType = "given_name",
        RoleClaimType = "role",
    };
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{Id?}");

app.Run();
