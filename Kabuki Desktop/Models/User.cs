using System;
using System.Collections.Generic;

namespace Kabuki_Desktop.Models
{
    public partial class User
    {
        public User()
        {
            Carts = new HashSet<Cart>();
            DiscountCards = new HashSet<DiscountCard>();
            Orders = new HashSet<Order>();
        }

        public int ID { get; set; }
        public string? Name { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? Password { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<DiscountCard> DiscountCards { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
