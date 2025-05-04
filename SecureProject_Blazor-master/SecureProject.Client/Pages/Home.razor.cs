using BlazorBootstrap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace SecureProject.Client.Pages
{
    [Authorize]
    public partial class Home
    {
        [Inject] protected ToastService ToastService { get; set; } = default!;
        [Inject] protected HttpClient _http { get; set; } = default!;
        
        private List<SecureProject.Shared.Product> products =null;
        bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            StateHasChanged(); // notify UI to refresh
            products = await _http.GetFromJsonAsync<List<Shared.Product>>("api/products") ?? new();
            isLoading = false;
            StateHasChanged(); // notify UI to refresh
        }

    }
}
