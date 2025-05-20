using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using SecureProject.Client.Auth;
using System.Security.Claims;

namespace SecureProject.Client.Layout
{
    public partial class MainLayout
    {
        [Inject] NavigationManager Navigation { get; set; }
        //[Inject] AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] CustomAuthenticationStateProvider CustomAuthenticationStateProvider { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }

        private ClaimsPrincipal? user;
        private bool isAdmin;
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
            var isAdminValue = user.Claims.FirstOrDefault(c => c.Type == "isadmin")?.Value ?? "false";
            isAdmin = Convert.ToBoolean(isAdminValue);
        }

        private async Task Logout()
        {
            await CustomAuthenticationStateProvider.Logout();
            Navigation.NavigateTo("/login", forceLoad: true);
        }
    }
}
