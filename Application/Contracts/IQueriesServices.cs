using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IQueriesServices
    {
        bool CountAllEntities();
        IEnumerable<object> GetFacultiesWithSupervisor();

        IEnumerable<object> GetCoursesWithFaculty();

        IEnumerable<object> AllCoursesInDecending();

        IEnumerable<object> AllCoursesWithNoOfInstructors();

        IEnumerable<object> AllClubsWithNoOfStudents();
    }
}
