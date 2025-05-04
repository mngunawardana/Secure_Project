using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SecureProject.Client.Auth;
using System.Security.Claims;

namespace SecureProject.Client.Layout
{
    public partial class MainLayout
    {
        [Inject] NavigationManager Navigation { get; set; }
        //[Inject] AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] CustomAuthenticationStateProvider CustomAuthenticationStateProvider { get; set; }

        private ClaimsPrincipal? user;
        private string? userName;
        protected override async Task OnInitializedAsync()
        {
            var authState = await CustomAuthenticationStateProvider.GetAuthenticationStateAsync();
            user = authState.User;

            if (!user.Identity.IsAuthenticated && !Navigation.Uri.Contains("login"))
            {
                // Redirect to login
                Navigation.NavigateTo("/login", forceLoad: true);
                return;
            }
            userName = user.Claims.First(c => c.Type == "unique_name").Value;
        }

        private async Task Logout()
        {
            await CustomAuthenticationStateProvider.Logout();

            Navigation.NavigateTo("/login", forceLoad: true);
        }
    }
}
