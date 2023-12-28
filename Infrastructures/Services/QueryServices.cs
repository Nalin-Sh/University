using Application.Contracts;
using Domain.Models;
using Infrastructures.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Services
{
    public class QueryServices : IQueriesServices
    {
        private readonly IGenericRepository<Club> _clubRepository;
        private readonly IGenericRepository<Student> _studenRepository;
        private readonly IGenericRepository<Faculty> _facultyRepository;
        private readonly IGenericRepository<Instructor> _instructorRepository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IGenericRepository<Courseassignment> _courseAssignment;

        public QueryServices(IGenericRepository<Club> clubRepository , 
            IGenericRepository<Student> studenRepository,
            IGenericRepository<Faculty> facultyRepository,
            IGenericRepository<Instructor> instructorRepository,
            IGenericRepository<Course> courseRepository,
            IGenericRepository<Courseassignment> courseAssignment) 
        {
            _clubRepository = clubRepository;
            _studenRepository = studenRepository;
            _facultyRepository = facultyRepository;
            _instructorRepository = instructorRepository;
            _courseRepository = courseRepository;
            _courseAssignment = courseAssignment;
        }
        public IEnumerable<object> AllClubsWithNoOfStudents()
        {
            var query = from Club in _clubRepository.GetAll()
                        join Students in _studenRepository.GetAll() on Club.Id equals Students.ClubId into studentClub
                        from sc in studentClub
                        select new
                        {
                            ClubName = Club.Entitle,
                            StudentCount = studentClub.Count(),
                            
                            
                        };
            return query.ToList();
        }

        public IEnumerable<object> AllCoursesInDecending()
        {
            var query = _courseRepository.GetAll();
                return query.ToList().OrderByDescending(x => x.Title);
        }

        public IEnumerable<object> AllCoursesWithNoOfInstructors()
        {
            var query = from Course in _courseRepository.GetAll()
                        join Courseassignment in _courseAssignment.GetAll() on Course.Id equals Courseassignment.CourseId into InstructorCourse
                        from ic in InstructorCourse
                        select new
                        {
                            Course = Course.Title,
                            Instructors = InstructorCourse.Distinct().Count()
                        };



            return query.ToList();
                        
        }

        public bool CountAllEntities()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetCoursesWithFaculty()
        {
            var query = from Course in _courseRepository.GetAll()
                        join Faculty in _facultyRepository.GetAll() on Course.FacultyId equals Faculty.Id into courseFaculty
                        from cf in courseFaculty
                        select new
                        {
                            Course = Course.Title,
                            Faculty = cf.Name
                        };

            return query.ToList();
         }

        public IEnumerable<object> GetFacultiesWithSupervisor()
        {
            var query = from Faculty in _facultyRepository.GetAll()
                        join Instructor in _instructorRepository.GetAll() on Faculty.SupervisorId equals Instructor.Id into facultySupervisor
                        from s in facultySupervisor
                        select new
                        {
                            Name = Faculty.Name,
                            SupervisorName = s.Name,

                        };

            return query.ToList();
                            
        }
    }
}
