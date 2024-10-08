﻿using System;
using System.Collections.Generic;

namespace LINQ_User.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? PersonId { get; set; }

    public virtual Person? Person { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
