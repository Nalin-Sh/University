using Application.Contracts;
using Domain.Models;
using Infrastructures.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Services
{
    public class LambdaServices : IQueriesServices
    {

        private readonly IGenericRepository<Club> _clubRepository;
        private readonly IGenericRepository<Student> _studenRepository;
        private readonly IGenericRepository<Faculty> _facultyRepository;
        private readonly IGenericRepository<Instructor> _instructorRepository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IGenericRepository<Courseassignment> _courseAssignment;
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Enrollment> _enrollmentRepository;

        public LambdaServices(IGenericRepository<Club> clubRepository,
            IGenericRepository<Student> studenRepository,
            IGenericRepository<Faculty> facultyRepository,
            IGenericRepository<Instructor> instructorRepository,
            IGenericRepository<Course> courseRepository,
            IGenericRepository<Courseassignment> courseAssignment,
            ApplicationDbContext context,
            IGenericRepository<Enrollment> enrollmentRepository)
        {
            _clubRepository = clubRepository;
            _studenRepository = studenRepository;
            _facultyRepository = facultyRepository;
            _instructorRepository = instructorRepository;
            _courseRepository = courseRepository;
            _courseAssignment = courseAssignment;
            _context = context;
            _enrollmentRepository = enrollmentRepository;
        }

        public IEnumerable<object> AllClubsWithNoOfStudents()
        {
            var query = _clubRepository.GetAll()
                        .GroupJoin(_studenRepository.GetAll(), club => club.Id, student => student.ClubId, 
                        (club, student) => new 
                        {
                            ClubName = club.Entitle,
                            StudentName = student.Count()
                        });

            return query.ToList();


        }

        public IEnumerable<object> AllCoursesInDecending()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> AllCoursesWithNoOfInstructors()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> AllFacultiesWithOrWithoutSupervisor()
        {
            throw new NotImplementedException();
        }

        public int CountAllEntities<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> CourseWithMarks()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetCoursesWithFaculty()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetFacultiesWithSupervisor()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetStudentsWithClubsAndCoursesWithFaculty()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetStudentsWithHighestAndLowestMarks()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> InstructorWithParticularCourse(string CourseName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> StrudentsWithTheirMarks()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> StudentEnrolledInParticularCourse(string course)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> StudentsWithHighestMarks()
        {
            throw new NotImplementedException();
        }
    }
}
