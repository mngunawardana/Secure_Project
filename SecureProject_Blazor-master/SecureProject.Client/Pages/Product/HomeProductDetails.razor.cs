using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor;
using SecureProject.Shared;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace SecureProject.Client.Pages.Product
{
    public partial class HomeProductDetails
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
                isPageLoading = false;
            }
        }
    }
}
