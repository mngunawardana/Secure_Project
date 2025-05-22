using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor;
using SecureProject.Shared;
using SecureProject.Shared.DTO;
using System.Net.Http.Json;
using static MudBlazor.CategoryTypes;
using static System.Net.WebRequestMethods;

namespace SecureProject.Client.Pages.Product
{
    public partial class ProductForm
    {
        [Parameter] public int? ProductId { get; set; }
        [Inject] private HttpClient Http { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        //[Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] protected ToastService ToastService { get; set; } = default!;

        private SecureProject.Shared.Product product = new();
        private SecureProject.Shared.DTO.ProductFeatureAddEditDTO feature = new();
        bool success;
        bool showAddFeature = false;
        bool isPageLoading = true;
        string[] errors = { };
        MudForm form;

        private bool IsEdit => ProductId.HasValue;

        protected override async Task OnInitializedAsync()
        {
            if (IsEdit)
            {
                product = await Http.GetFromJsonAsync<SecureProject.Shared.Product>($"api/products/{ProductId}") ?? new();
            }
            isPageLoading = false;

        }
        private void OnValidSubmit(EditContext context)
        {
            success = true;
            StateHasChanged();
        }
        private async Task HandleValidSubmit()
        {
            isPageLoading = true;
            StateHasChanged();

            if (IsEdit)
            {
            
                var productDb = new SecureProject.Shared.Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    VendorName = product.VendorName,
                    VendorLogo = product.VendorLogo
                };
                await Http.PutAsJsonAsync($"api/products/{product.Id}", productDb);
                ToastService.Notify(new(ToastType.Success, $"Product successfullyc updeted. Please add features"));
            }
            else
            {
                var productDb = new SecureProject.Shared.Product
                {
                    Id = 0,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    VendorName = product.VendorName,
                    VendorLogo = product.VendorLogo
                };

                //await Http.PostAsJsonAsync("api/products", productDb);
                var response = await Http.PostAsJsonAsync("api/products", productDb);

                if (response.IsSuccessStatusCode)
                {
                    var createdProduct = await response.Content.ReadFromJsonAsync<Shared.Product>();
                   product.Id= createdProduct?.Id ?? 0;
                    ProductId = product.Id;
                    ToastService.Notify(new(ToastType.Success, $"Product successfully updeted. Please add features"));

                }
                else
                {
                    
                    ToastService.Notify(new(ToastType.Danger, $"Product successfully updeted. Please add features"));

                }
                //Snackbar.Add("Product successfully added. Please add features", Severity.Success);
                ToastService.Notify(new(ToastType.Success, $"Product successfully added. Please add features"));
            }
            //Navigation.NavigateTo("/products");
            isPageLoading = false;

        }

        private async Task FetureValidSubmit()
        {
            isPageLoading = true;
            StateHasChanged();

            if (feature.Id > 0 && product.ProductFeatures.Any(c => c.Id == feature.Id))
            {
                var featureDb = product.ProductFeatures.First(c => c.Id == feature.Id);
                feature.Id = featureDb.Id;
                feature.ProductId = product.Id;
                showAddFeature = false;
                await Http.PutAsJsonAsync($"api/productFeature/{featureDb.Id}", feature);
                //Snackbar.Add("Feture successfully updeted. Please add features", Severity.Success);
                ToastService.Notify(new(ToastType.Success, $"Product successfully updeted. Please add features"));

            }
            else
            {

                feature.ProductId = product.Id;
                await Http.PostAsJsonAsync("api/productFeature", feature);
                showAddFeature = false;

                ToastService.Notify(new(ToastType.Success, $"Product successfully added. Please add features"));

            }
            product = await Http.GetFromJsonAsync<SecureProject.Shared.Product>($"api/products/{product.Id}") ?? new();

            isPageLoading = false;

        }

        private async Task HandleLogoUpload(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null)
            {
                using var stream = file.OpenReadStream(5 * 1024 * 1024); // limit to 5MB
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                product.VendorLogo = ms.ToArray();
            }
        }

        private void AddFeature()
        {
            showAddFeature = true;
            feature.OrderBy = product.ProductFeatures.ToList().Count() + 1;
            feature.Name = "";
            feature.Value = "";
            feature.Id = 0;
        }

        private void EditFeature(ProductFeature selected)
        {
            showAddFeature = true;
            feature.OrderBy = selected.OrderBy;
            feature.Name = selected.Name;
            feature.Value = selected.Value;
            feature.Id = selected.Id;
        }
        private async Task DeleteFeature(ProductFeature selected)
        {
            isPageLoading = true;
            StateHasChanged();
            await Http.DeleteAsync($"api/productFeature/{selected.Id}");
            //Snackbar.Add("Feature successfully added", Severity.Success);
            ToastService.Notify(new(ToastType.Success, $"Feature successfully deleted"));

            isPageLoading = false;
        }


        private void Cancel()
        {
            Navigation.NavigateTo("/products");
        }
    }
}
