using System;
using System.Collections.Generic;

namespace WPF_User.Models
{
    public partial class Order
    {
        public Order()
        {
            Products = new HashSet<Product>();
        }

        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? PersonId { get; set; }

        public virtual Person? Person { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
