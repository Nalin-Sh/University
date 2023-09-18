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
    public class CourseAssignmentServices
    {
        private readonly IGenericRepository<Courseassignment> _courseAssignmentRepository;
        private readonly IGenericRepository<Instructor> _instructorRepository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly ApplicationDbContext _context;

        public CourseAssignmentServices(IGenericRepository<Courseassignment> courseAssignmentRepository,
                                        ApplicationDbContext context,
                                        IGenericRepository<Course> courseRepository,
                                        IGenericRepository<Instructor> instructorRepository)
        {
            _courseAssignmentRepository = courseAssignmentRepository;
            _context = context;
            _instructorRepository = instructorRepository;
            _courseRepository = courseRepository;
        }
        public bool AssignCourseToInstructor(CourseAssignmentRequest request)
        {
            var instructor = _instructorRepository.GetAll().FirstOrDefault(i=> i.Id == request.InstructorId);
            if (instructor == null)
            {
                throw new InvalidOperationException("Instructor Not Found");
            }
            var course = _courseRepository.GetAll().FirstOrDefault(i=> i.Id == request.CourseId);
            if (course == null)
            {
                throw new InvalidOperationException("Course Not Found");
            }
            var existingAssignment = _context.Courseassignments.FirstOrDefault(ca => ca.CourseId == request.CourseId && ca.InstructorId == request.InstructorId);
            if (existingAssignment != null) 
            {
                return false;
            }
            var assignment = new Courseassignment
            {
                CourseId = request.CourseId,
                InstructorId = request.InstructorId
            };
            var result = _courseAssignmentRepository.Add(assignment);
            return result;

        }

        public IEnumerable<CourseAssignmentResponse> GetAll()
        {
            var courseAssignment = _courseAssignmentRepository.GetAll();
            var instructor = _instructorRepository.GetAll();
            var courses = _courseRepository.GetAll();
            var result = from Courseassignment in courseAssignment
                         join Course in courses
                         on Courseassignment.CourseId equals Course.Id
                         join Instructor in instructor
                         on Courseassignment.InstructorId equals Instructor.Id
                         select new CourseAssignmentResponse
                         {
                             InstructorId = Instructor.Id,
                             InstructorName = Instructor.Name,
                             CourseId = Course.Id,
                             CourseName = Course.Title

                         };
            return result.ToList();
        }
        public bool UpdateCourseAssignment(CourseAssignmentRequest request)
        {
            var existingAssignment = _context.Courseassignments.FirstOrDefault(ca => ca.CourseId == request.CourseId && ca.InstructorId == request.InstructorId);
            if (existingAssignment == null)
            {
                throw new InvalidOperationException("Course Assignment Not Found");
            }
            var result = _courseAssignmentRepository.Update(existingAssignment);
            return result ; 
        }
        public bool DeleteCourseAssignment(CourseAssignmentRequest request)
        {
            var existingAssignment = _context.Courseassignments.FirstOrDefault(ca => ca.CourseId == request.CourseId && ca.InstructorId == request.InstructorId);
            if (existingAssignment == null)
            {
                throw new InvalidOperationException("Course Assignment Not Found");
            }
            var result = _courseAssignmentRepository.Delete(existingAssignment);
            return result;
        }
    }
}
