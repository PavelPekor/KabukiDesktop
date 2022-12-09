using Kabuki_Desktop.ViewModels;
using System;
using System.Collections.Generic;

namespace Kabuki_Desktop.Models
{
    public partial class Cart : BaseVM
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }

        private int? quantity;
        public int? Quantity
        {
            get => quantity;
            set { quantity = value; OnPropertyChanged(); }
        }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
