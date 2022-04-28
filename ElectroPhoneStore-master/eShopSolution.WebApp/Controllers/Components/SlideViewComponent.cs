using eShopSolution.ApiIntegration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Controllers.Components
{
    public class SlideViewComponent : ViewComponent
    {
        private readonly ISlideApiClient _slideApiClient;

        public SlideViewComponent(ISlideApiClient slideApiClient)
        {
            _slideApiClient = slideApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryList = await _slideApiClient.GetAll();

            return View("Default", categoryList);
        }
    }
    
}
