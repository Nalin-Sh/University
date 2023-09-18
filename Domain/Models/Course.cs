using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Course
    {
        public Course()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Credits { get; set; }
        public int? FacultyId { get; set; }

        public virtual Faculty? Faculty { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
