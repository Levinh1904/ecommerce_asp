using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Producers
{
    public class ProducerCreateRequest
    {
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }

        [Display(Name = "Hình ảnh")]
        public IFormFile Image { get; set; }
    }
}
