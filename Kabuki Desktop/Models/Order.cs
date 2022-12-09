using System;
using System.Collections.Generic;

namespace Kabuki_Desktop.Models
{
    public partial class Order
    {
        public Order()
        {
            ProductsInOrders = new HashSet<ProductsInOrder>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Address { get; set; }
        public DateTime? DateTimeOrder { get; set; }
        public int? OrderPrice { get; set; }
        public bool? IsDiscountCard { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<ProductsInOrder> ProductsInOrders { get; set; }
    }
}
