using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace SecureProject.Client.Service
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }
    }    }