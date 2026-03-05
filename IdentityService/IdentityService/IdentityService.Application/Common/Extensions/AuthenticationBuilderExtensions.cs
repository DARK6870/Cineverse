using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using IdentityService.Application.Common.Models.Options;
using IdentityService.Mongo.Schemas.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application.Common.Extensions;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddExternalProviders(
        this AuthenticationBuilder authBuilder,
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        var externalProvidersOptionsSection = configuration
            .GetSection(nameof(ExternalProvidersOptions));
        
        var externalProvidersOptions = externalProvidersOptionsSection.Get<ExternalProvidersOptions>()
            ?? throw new InvalidOperationException("Missing ExternalProvidersOptions");
        
        services.Configure<ExternalProvidersOptions>(externalProvidersOptionsSection);
        
        authBuilder.AddCookie();
        authBuilder.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.ClientId = externalProvidersOptions.Google.ClientId;
            options.ClientSecret = externalProvidersOptions.Google.ClientSecret;
            options.CallbackPath = "/signin-google";
            options.CorrelationCookie.SameSite = SameSiteMode.Lax;
            options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Scope.Add("profile");
            options.Scope.Add("email");
            options.SaveTokens = true;
            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            options.ClaimActions.MapJsonKey("picture", "picture");
        });

        authBuilder.AddOAuth(nameof(AuthenticationProvider.Github), options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.ClientId = externalProvidersOptions.GitHub.ClientId;
            options.ClientSecret = externalProvidersOptions.GitHub.ClientSecret;
            options.CallbackPath = "/signin-github";
            options.CorrelationCookie.SameSite = SameSiteMode.Lax;
            options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
            options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
            options.TokenEndpoint = "https://github.com/login/oauth/access_token";
            options.UserInformationEndpoint = "https://api.github.com/user";
            options.Scope.Add("user:email");
            options.SaveTokens = true;
            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            options.ClaimActions.MapJsonKey("avatar_url", "avatar_url");
            options.ClaimActions.MapJsonKey("login", "login");
            options.Events = new OAuthEvents
            {
                OnCreatingTicket = async context =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                    request.Headers.UserAgent.Add(new ProductInfoHeaderValue("Cineverse", "1.0"));

                    var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                    response.EnsureSuccessStatusCode();

                    var user = await response.Content.ReadFromJsonAsync<JsonElement>();
                    context.RunClaimActions(user);

                    var emailClaim = context.Principal?.FindFirst(ClaimTypes.Email);
                    if (emailClaim == null || string.IsNullOrEmpty(emailClaim.Value))
                    {
                        var emailRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/emails");
                        emailRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        emailRequest.Headers.Authorization =
                            new AuthenticationHeaderValue("Bearer", context.AccessToken);
                        emailRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue("Cineverse", "1.0"));

                        var emailResponse =
                            await context.Backchannel.SendAsync(emailRequest, context.HttpContext.RequestAborted);
                        emailResponse.EnsureSuccessStatusCode();

                        var emails = await emailResponse.Content.ReadFromJsonAsync<JsonElement>();

                        var primaryEmail = emails.EnumerateArray()
                            .FirstOrDefault(e =>
                                e.GetProperty("primary").GetBoolean() &&
                                e.GetProperty("verified").GetBoolean());

                        if (primaryEmail.ValueKind != JsonValueKind.Undefined)
                        {
                            var email = primaryEmail.GetProperty("email").GetString();
                            context.Identity?.AddClaim(new Claim(ClaimTypes.Email, email!));
                        }
                    }
                }
            };
        });

        return authBuilder;
    }
}