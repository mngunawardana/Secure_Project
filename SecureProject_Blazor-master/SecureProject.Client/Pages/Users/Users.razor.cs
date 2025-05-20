using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace SecureProject.Client.Pages.Users
{
    public partial class Users
    {
        [Inject] protected ToastService ToastService { get; set; } = default!;
        private List<SecureProject.Shared.ApplicationUser> users = new();
        bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            StateHasChanged(); // notify UI to refresh
            users = await Http.GetFromJsonAsync<List<Shared.ApplicationUser>>("api/auth/userList") ?? new();
            isLoading = false;
            StateHasChanged(); // notify UI to refresh
        }

        private async Task GrantAdmin(string id)
        {
            await Http.GetFromJsonAsync<bool>($"api/auth/grantadmin?id={id}");
            users = await Http.GetFromJsonAsync<List<Shared.ApplicationUser>>("api/auth/userList") ?? new();

        }
        private async Task  RevokeAdmin(string id) 
        {
            await Http.GetFromJsonAsync<bool>($"api/auth/revokeAdmin?id={id}");
        users = await Http.GetFromJsonAsync<List<Shared.ApplicationUser>>("api/auth/userList") ?? new ();
        }
}
}
