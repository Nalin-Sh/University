using Application.Contracts;
using Application.RequestModels;
using Application.ResponseModels;
using Domain.Models;
using Infrastructures.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Services
{
    public class CourseServices
    {
        private readonly IGenericRepository<Faculty> _facultyRepository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly ApplicationDbContext _context;

        public CourseServices(IGenericRepository<Faculty> facultyRepository, IGenericRepository<Course> courseRepository, ApplicationDbContext context)
        {
            _facultyRepository = facultyRepository;
            _courseRepository = courseRepository;
            _context = context;
        }

        public bool AddCourse(string courseName, int facultyId, int credits)
        {
            var faculty = _facultyRepository.GetAll().FirstOrDefault(i => i.Id == facultyId);
            if (faculty == null)
            {
                throw new InvalidOperationException("Faculty not found");
            }
            else
            {
                var newCourse = new Course()
                {
                    Title = courseName,
                    Credits = credits,
                    FacultyId = facultyId,
                    Id = (_context.Courses.ToList().Max(x => x.Id)) + 1
                };
                var result = _courseRepository.Add(newCourse);
                return result;
            }
        }
        public IEnumerable<CourseResponse> GetCourses()
        {
            var course = _courseRepository.GetAll();
            var faculty = _facultyRepository.GetAll();

            var result = from Course in course
                         join Faculty in faculty
                         on Course.FacultyId equals Faculty.Id into CourseFaculty
                         from courseFaculty in CourseFaculty.DefaultIfEmpty()
                         select new CourseResponse
                         {
                             CourseId = Course.Id,
                             CourseName = Course.Title,
                             Credits = Course.Credits,
                             FacultyID = courseFaculty.Id,
                             FacultyName = courseFaculty.Name
                         };
            return result.ToList();
        }
        public bool DeleteCourse(int id)
        {
            var courseToDelete = _context.Courses.FirstOrDefault(f => f.Id == id);
            bool deleted = _courseRepository.Delete(courseToDelete);
            if (deleted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateCourse(int id, CourseRequest request)
        {

            var course = _context.Courses.FirstOrDefault(f => f.Id == id);
            if (course == null)
            {
                throw new InvalidOperationException("Course not found");
            }
            course.Title = request.Title;
            course.FacultyId = request.FacultyId;
            course.Credits = request.Credits;
            _context.SaveChanges();
            return true;
        }
    }
}
