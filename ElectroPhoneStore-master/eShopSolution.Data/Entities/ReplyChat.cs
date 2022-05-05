using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class ReplyChat
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Reply")]
        public string Reply { get; set; }
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
