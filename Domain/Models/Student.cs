using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Student
    {
        public Student()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public DateOnly EnrollmentDate { get; set; }
        public int? ClubId { get; set; }

        public virtual Club? Club { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
