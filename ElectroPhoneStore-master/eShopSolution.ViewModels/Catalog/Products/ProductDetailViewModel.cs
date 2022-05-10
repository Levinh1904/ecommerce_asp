using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Producers;
using eShopSolution.ViewModels.Catalog.ProductImages;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductDetailViewModel
    {
        public CategoryViewModel Category { get; set; }
        public ProducerViewModel Producer { get; set; }
        public ProductViewModel Product { get; set; }

        //public List<ReviewViewModel> Reviews { get; set; }

        public List<ReviewViewModel> ListOfReviews { get; set; }
        public string Review { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }

        // Use to get user id when add review
        public string UserCommentId { get; set; }

        public List<ViewModels.Catalog.Products.ProductViewModel> FeaturedProducts { get; set; }
        public List<ProductImageViewModel> ProductImages { get; set; }
    }
}