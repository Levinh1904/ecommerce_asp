using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public int CategoryId { get; set; }
        public int ProducerId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { set; get; }
        public string Details { set; get; }

        // thumbnail path
        public string Thumbnail { get; set; }

        // product image path
        public string ProductImage { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }


        public Category Category { get; set; }
        public Producer Producer { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public string Status { set; get; }
    }
}