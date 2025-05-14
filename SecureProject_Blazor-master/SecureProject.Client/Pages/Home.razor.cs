using BlazorBootstrap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace SecureProject.Client.Pages
{
    [Authorize]
    public partial class Home
    {
        [Inject] protected ToastService ToastService { get; set; } = default!;
        [Inject] protected HttpClient _http { get; set; } = default!;
        [Inject] protected IJSRuntime JS { get; set; } = default!;

        private List<SecureProject.Shared.Product> products = null;
        private List<SecureProject.Shared.Product> selectedProducts = new List<Shared.Product>();
        bool isLoading;
        private List<string> featureList = new();
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            StateHasChanged(); // notify UI to refresh
            products = await _http.GetFromJsonAsync<List<Shared.Product>>("api/products") ?? new();
            isLoading = false;
            ChangeSelectedItem();

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

        private async Task PrintDivAsync()
        {
            await JS.InvokeVoidAsync("printDiv", "divComparison");
        }
    }
}
