using System;
using System.Collections.Generic;

namespace WPF_User.Models
{
    public partial class Person
    {
        public Person()
        {
            Orders = new HashSet<Order>();
            Hobbies = new HashSet<Hobby>();
        }

        public int PersonId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? AddressId { get; set; }
        public string? Gender { get; set; }

        public virtual Address? Address { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Hobby> Hobbies { get; set; }
    }
}
