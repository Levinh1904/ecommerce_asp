using eShopSolution.ApiIntegration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Controllers.Components
{
    public class ContactViewComponent : ViewComponent
    {
        private readonly IContactApiClient _contactApiClient;

        public ContactViewComponent(IContactApiClient contactApiClient)
        {
            _contactApiClient = contactApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryList = await _contactApiClient.GetAll();

            return View("Default", categoryList);
        }
    }

}
