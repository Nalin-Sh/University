using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IQueriesServices
    {
        int CountAllEntities<T>() where T : class;
        IEnumerable<object> GetFacultiesWithSupervisor();

        IEnumerable<object> GetCoursesWithFaculty();

        IEnumerable<object> AllCoursesInDecending();

        IEnumerable<object> AllCoursesWithNoOfInstructors();

        IEnumerable<object> AllClubsWithNoOfStudents();

        IEnumerable<object> AllFacultiesWithOrWithoutSupervisor();

        IEnumerable<object> InstructorWithParticularCourse(string CourseName);

        IEnumerable<object> StudentEnrolledInParticularCourse(string course);

        IEnumerable<object> CourseWithMarks();

        IEnumerable<object> StudentsWithHighestMarks();

        IEnumerable<object> StrudentsWithTheirMarks();

        IEnumerable<object> GetStudentsWithClubsAndCoursesWithFaculty();

        IEnumerable<object> GetStudentsWithHighestAndLowestMarks();
    }
}
