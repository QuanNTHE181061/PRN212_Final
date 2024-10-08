using System;
using System.Collections.Generic;

namespace WPF_User.Models
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
