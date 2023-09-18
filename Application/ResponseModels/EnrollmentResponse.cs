using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ResponseModels
{
    public class EnrollmentResponse
    {
        public int EnrollmentId { get; set; }
        public DateOnly EntollmentDate { get; set; }

        public int Marks { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public int StudentId { get; set; }
        public string StudentName { get; set;}
    }
}
