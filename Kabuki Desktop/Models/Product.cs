using System;
using System.Collections.Generic;

namespace Kabuki_Desktop.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            ProductsInOrders = new HashSet<ProductsInOrder>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Price { get; set; }
        public string? Photo { get; set; }
        public string? Description { get; set; }
        public int? Category { get; set; }

        public virtual Category? CategoryNavigation { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<ProductsInOrder> ProductsInOrders { get; set; }
    }
}
