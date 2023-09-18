using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ResponseModels
{
    public class FacultyResponse
    {
        public int FacultyId { get; set; }
        public string FacultyName { get; set; }
        public int SupervisorID { get; set; }
        public string SupervisorName { get; set;}
    }
}
