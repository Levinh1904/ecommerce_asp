using eShopSolution.ViewModels.Catalog.Producers;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Models
{
    public class ProductProducerViewModel:PagingRequestBase
    {
        public ProducerViewModel Producer { get; set; }

        public PagedResult<ProductViewModel> Products { get; set; }
    }

}
