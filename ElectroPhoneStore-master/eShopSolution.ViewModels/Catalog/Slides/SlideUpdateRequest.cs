using eShopSolution.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Slides
{
    // thường ta chỉ update các property trong translation
    public class SlideUpdateRequest
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name = "Tiêu đề")]
        public string Tille { get; set; }

        [Display(Name = "Màu")]
        public string Color { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public IFormFile Image { get; set; }


        [Display(Name = "Màu")]
        public string Status { get; set; }
    }
}
