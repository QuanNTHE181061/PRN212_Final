using System;
using System.Collections.Generic;

namespace LINQ_User.Models;

public partial class Hobby
{
    public int HobbyId { get; set; }

    public string? HobbyName { get; set; }

    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
