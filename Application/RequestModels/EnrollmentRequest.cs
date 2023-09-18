using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RequestModels
{
    public class EnrollmentRequest
    {
        public int Marks { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }

    }
}
