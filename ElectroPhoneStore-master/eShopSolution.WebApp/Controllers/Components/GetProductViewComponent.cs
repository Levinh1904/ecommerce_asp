using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Controllers.Components
{
    public class GetProductViewComponent : ViewComponent
    {
        private readonly IProductApiClient _productApiClient;

        public GetProductViewComponent(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var culture = CultureInfo.CurrentCulture.Name;
            var producerList = await _productApiClient.GetFeaturedProducts(culture, SystemConstants.ProductSettings.NumberOfFeturedProducts);

            return View("Default", producerList);
        }
    }
}
