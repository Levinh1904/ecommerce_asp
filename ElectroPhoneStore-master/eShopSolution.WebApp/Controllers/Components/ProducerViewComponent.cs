using eShopSolution.ApiIntegration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Controllers.Components
{
    public class ProducerViewComponent : ViewComponent
    {
        private readonly IProducerApiClient _producerApiClient;

        public ProducerViewComponent(IProducerApiClient producerApiClient)
        {
            _producerApiClient = producerApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var producerList = await _producerApiClient.GetAll();

            return View("Default", producerList);
        }
    }
    
}
