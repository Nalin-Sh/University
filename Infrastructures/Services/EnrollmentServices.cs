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
    public class EnrollmentServices
    {
        private readonly IGenericRepository<Enrollment> _enrollmentRepository;
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly ApplicationDbContext _context;

        public EnrollmentServices(IGenericRepository<Enrollment> enrollmentRepository,
                                        ApplicationDbContext context,
                                        IGenericRepository<Course> courseRepository,
                                        IGenericRepository<Student> studentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _context = context;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        public bool EnrollStudent(EnrollmentRequest request)
        {
            var student = _studentRepository.GetAll().FirstOrDefault(i => i.StudentId == request.StudentId);
            if (student == null)
            {
                throw new InvalidOperationException("Student Not Found");
            }
            var course = _courseRepository.GetAll().FirstOrDefault(i => i.Id == request.CourseId);
            if (course == null)
            {
                throw new InvalidOperationException("Course Not Found");
            }
            var existingEnrollment = _context.Enrollments.FirstOrDefault(ca => ca.CourseId == request.CourseId && ca.StudentId == request.StudentId);
            if (existingEnrollment != null)
            {
                return false;
            }
            var enrollment = new Enrollment
            {
                Id = (_context.Enrollments.ToList().Max(x => x.Id)) + 1,
                CourseId = request.CourseId,
                StudentId = request.StudentId,
                Marks = request.Marks,
            };
            var result = _enrollmentRepository.Add(enrollment);
            return result;
        }
        public IEnumerable<EnrollmentResponse> GetAll()
        {
            var enrollment = _enrollmentRepository.GetAll();
            var student = _studentRepository.GetAll();
            var courses = _courseRepository.GetAll();
            var result = from Enrollment in enrollment
                         join Course in courses
                         on Enrollment.CourseId equals Course.Id
                         join Student in student
                         on Enrollment.StudentId equals Student.StudentId
                         select new EnrollmentResponse
                         {
                             EnrollmentId = Enrollment.Id,
                             StudentId = Student.StudentId,
                             StudentName = Student.StudentName,
                             Marks = Enrollment.Marks,
                             EntollmentDate = Student.EnrollmentDate,
                             CourseId = Course.Id,
                             CourseName = Course.Title
                         };
            return result.ToList();
        }
        public bool UpdateEnrollment(EnrollmentRequest request)
        {
            var existingEnrollment = _context.Enrollments.FirstOrDefault(ca => ca.CourseId == request.CourseId && ca.StudentId == request.StudentId);
            if (existingEnrollment == null)
            {
                throw new InvalidOperationException("Enrollment Not Found");
            }
            var result = _enrollmentRepository.Update(existingEnrollment);
            return result;
        }
        public bool DeleteEnrollment(EnrollmentRequest request)
        {
            var existingEnrollment = _context.Enrollments.FirstOrDefault(ca => ca.CourseId == request.CourseId && ca.StudentId == request.StudentId);
            if (existingEnrollment == null)
            {
                throw new InvalidOperationException("Enrollment Not Found");
            }
            var result = _enrollmentRepository.Delete(existingEnrollment);
            return result;
        }
    }

}
