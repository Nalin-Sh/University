using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RequestModels
{
    public class CourseAssignmentRequest
    {
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
    }
}
