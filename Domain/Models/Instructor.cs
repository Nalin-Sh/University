using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public partial class Instructor
    {
        public Instructor()
        {
            Faculties = new HashSet<Faculty>();
        }
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Faculty> Faculties { get; set; }
    }
}
