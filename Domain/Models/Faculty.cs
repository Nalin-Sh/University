using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public partial class Faculty
    {
        public Faculty()
        {
            Courses = new HashSet<Course>();
        }
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? SupervisorId { get; set; }

        public virtual Instructor? Supervisor { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
