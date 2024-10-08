using System;
using System.Collections.Generic;

namespace WPF_User.Models
{
    public partial class Address
    {
        public Address()
        {
            People = new HashSet<Person>();
        }

        public int AddressId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }

        public virtual ICollection<Person> People { get; set; }
    }
}
