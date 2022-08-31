using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Producer
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
