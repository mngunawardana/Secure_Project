using BlazorBootstrap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using SecureProject.Client.Auth;
using SecureProject.Client.Service;
using SecureProject.Shared;
using SecureProject.Shared.DTO;
using System.Net.Http.Json;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace SecureProject.Client.Pages
{
    [Authorize]
    public partial class Home
    {
        [Inject] protected ToastService ToastService { get; set; } = default!;
        [Inject] protected HttpClient _http { get; set; } = default!;
        [Inject] protected IJSRuntime JS { get; set; } = default!;
        [Inject] protected ProductRecommendationService _recommendationService { get; set; } = default!;
        [Inject] CustomAuthenticationStateProvider CustomAuthenticationStateProvider { get; set; }
        [Inject] NavigationManager Navigation { get; set; }

        private ClaimsPrincipal? user;
        private List<SecureProject.Shared.Product> products = null;
        private List<SecureProject.Shared.Product> selectedProducts = new List<Shared.Product>();
        private RecommendationDTO recommendations;

        private List<string> featureList = new();
        private Modal modal = default!;
        bool isLoading;

        string apiKey = "AIzaSyAoulJ9ZSsDVlDhwY5rEsr35MWQK1mbe7U";
        //string apiKey = "AIzaSyA77B0VlB9RdqLY0AAvG1QpZcIHy8r35hw";
        string userRequest = "";

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

            isLoading = true;
            StateHasChanged(); // notify UI to refresh
            products = await _http.GetFromJsonAsync<List<Shared.Product>>("api/products") ?? new();



            isLoading = false;
            ChangeSelectedItem();

            StateHasChanged(); // notify UI to refresh
        }

        protected async Task GetRecomondation()
        {
            isLoading = true;
            StateHasChanged(); 

            var recommendationService = new GeminiProductRecommendationService(apiKey, products);
            recommendations = await recommendationService.GetRecommendations(userRequest);

            ChangeSelectedItem();
            isLoading = false;

            StateHasChanged(); // notify UI to refresh
        }


        protected async Task SelectItem(Shared.Product product)
        {
            product.IsSelected = !product.IsSelected;
            ChangeSelectedItem();
        }

        protected void ChangeSelectedItem()
        {
            selectedProducts = products.Where(c => c.IsSelected).ToList();
            foreach (var product in selectedProducts)
            {
                foreach (var feature in product.ProductFeatures.OrderBy(o => o.OrderBy))
                {
                    var name = feature.Name.ToUpper().Trim();
                    feature.Name = name;
                    if (!featureList.Any(c => c == name))
                    {
                        featureList.Add(name); // preserve order
                    }
                }
            }
            StateHasChanged(); // notify UI to refresh
        }

        private async Task PrintComparisionDivAsync()
        {
            await JS.InvokeVoidAsync("printDiv", "divComparison");
        }

        private async Task PrintRecomondationDivAsync()
        {
            await JS.InvokeVoidAsync("printDiv", "divRecomondation");
        }


        private async Task OnShowModalClick()
        {
            await modal.ShowAsync();
        }

        private async Task OnHideModalClick()
        {
            await modal.HideAsync();
        }
    }
}
