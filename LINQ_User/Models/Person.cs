using System;
using System.Collections.Generic;

namespace LINQ_User.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public int? AddressId { get; set; }

    public string? Gender { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Hobby> Hobbies { get; set; } = new List<Hobby>();
}
