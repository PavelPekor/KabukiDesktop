using System;
using System.Collections.Generic;

namespace Kabuki_Desktop.Models
{
    public partial class DiscountCard
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? NumberCard { get; set; }

        public virtual User? User { get; set; }
    }
}
