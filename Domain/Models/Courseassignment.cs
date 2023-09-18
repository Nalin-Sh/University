using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Courseassignment
    {
        public int InstructorId { get; set; }
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}
