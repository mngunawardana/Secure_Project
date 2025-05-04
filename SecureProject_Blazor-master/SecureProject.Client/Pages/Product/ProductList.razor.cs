using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace SecureProject.Client.Pages.Product
{
    public partial class ProductList
    {
        [Inject] protected ToastService ToastService { get; set; } = default!;
        private List<SecureProject.Shared.Product> products = new();
        bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            StateHasChanged(); // notify UI to refresh
            products = await Http.GetFromJsonAsync<List<Shared.Product>>("api/products") ?? new();
            isLoading = false;
            StateHasChanged(); // notify UI to refresh
        }

        private void AddProduct() => Navigation.NavigateTo("/products/new");
        private void EditProduct(int id) => Navigation.NavigateTo($"/products/edit/{id}");
        private async Task DeleteProduct(int id)
        {
            await Http.DeleteAsync($"api/products/{id}");
            ToastService.Notify(new(ToastType.Success, $"Product successfully deleted"));

            products = await Http.GetFromJsonAsync<List<Shared.Product>>("api/products") ?? new();
        }
    }
}
