using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Domain.Models
{
    public partial class Club
    {
        public Club()
        {
            Students = new HashSet<Student>();
        }
        [Key]
        public int Id { get; set; }
        public string? Entitle { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
