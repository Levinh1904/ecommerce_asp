﻿using eShopSolution.ViewModels.Catalog.Categories;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Models
{
    public class HomeViewModel
    {
        public List<ViewModels.Catalog.Products.ProductViewModel> FeaturedProducts { get; set; }

        public List<ViewModels.Catalog.Products.ProductViewModel> LatestProducts { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}
