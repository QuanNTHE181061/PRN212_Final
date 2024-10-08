using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_User.Models
{
    public partial class Hobby
    {
        public Hobby()
        {
            People = new HashSet<Person>();
        }

        public int HobbyId { get; set; }
        public string? HobbyName { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; } // Thêm thuộc tính IsSelected và đánh dấu là không ánh xạ
        public virtual ICollection<Person> People { get; set; }
    }
}
