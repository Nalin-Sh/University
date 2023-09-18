using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ResponseModels
{
    public class CourseResponse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int? Credits { get; set; }
        public int FacultyID { get; set; }
        public string FacultyName { get; set; }
    }
}
