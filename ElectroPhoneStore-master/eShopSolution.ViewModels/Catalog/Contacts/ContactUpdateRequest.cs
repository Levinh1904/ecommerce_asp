using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Contacts
{
    public class ContactUpdateRequest
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name = "Nội dung")]
        public string Tille { get; set; }
        [Display(Name = "Gmail")]
        public string Gmail { get; set; }

        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public IFormFile Image { get; set; }


        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }
    }
}
