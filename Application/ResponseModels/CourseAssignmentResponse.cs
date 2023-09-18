using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ResponseModels
{
    public class CourseAssignmentResponse
    {
        public int InstructorId { get; set; }
        public string InstructorName { get; set;}
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
