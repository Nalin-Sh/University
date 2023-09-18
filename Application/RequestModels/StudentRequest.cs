using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RequestModels
{
    public class StudentRequest
    {
        public string Name { get; set; }
        public int ClubID { get; set; }
        public DateTime EnrolledDate { get; set; }
    }
}
