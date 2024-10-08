using System;
using System.Collections.Generic;

namespace LINQ_User.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? ZipCode { get; set; }

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
