using System;
using System.Collections.Generic;

namespace LINQ_User.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
