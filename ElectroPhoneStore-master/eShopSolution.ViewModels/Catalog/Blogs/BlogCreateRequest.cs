using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Blogs
{
    public class BlogCreateRequest
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name = "Tiêu đề")]
        public string Tille { get; set; }
        [Display(Name = "Nội dung")]
        public string Details { get; set; }

        [Display(Name = "Ảnh")]
        public IFormFile Image { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime Starttime { get; set; }
        [Display(Name = "Trạng Thái")]
        public bool Status { get; set; }
    }
}
