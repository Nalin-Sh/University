using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ResponseModels
{
    public class StudentResponse
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateOnly EnrolledDate { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }
    }
}
