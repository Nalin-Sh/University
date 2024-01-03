using Application.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueriesController : ControllerBase
    {
        private readonly IQueriesServices _queriesServices;

        public QueriesController(IQueriesServices queriesServices) 
        {
            _queriesServices = queriesServices;
        }

        [HttpGet]
        public IActionResult GetAllFacultyWithSupervisor()
        {
            var result = _queriesServices.GetFacultiesWithSupervisor();
            return Ok(result);

        }

        [HttpGet("Courses-with_Faculty")]
        public IActionResult GetCoursesWithFaculty()
        {
            var result = _queriesServices.GetCoursesWithFaculty();
            return Ok(result);

        }

        [HttpGet("Clubs-with-Students-Count")]
        public IActionResult GetStudentsWithClub()
        {
            var result = _queriesServices.AllClubsWithNoOfStudents();
            return Ok(result);
        }

        [HttpGet("Courses-Decending")]
        public IActionResult GetCoursesDecending()
        {
            var result = _queriesServices.AllCoursesInDecending();
            return Ok(result);
        }

        [HttpGet("All-Courses-With-No-Of-Instructors")]
        public IActionResult GetCoursesWithInstructorCount()
        {
            var result = _queriesServices.AllCoursesWithNoOfInstructors();
            return Ok(result);
        }

        [HttpGet("Count-of-All_Entities")]
        public IActionResult CountAllEntities()
        {
            var counts = new
            {
                courseCount = _queriesServices.CountAllEntities<Course>(),
                StudentCount = _queriesServices.CountAllEntities<Student>(),
                FaultyCount = _queriesServices.CountAllEntities<Faculty>(),
                ClubCount = _queriesServices.CountAllEntities<Club>(),
                InstructorCount = _queriesServices.CountAllEntities<Instructor>(),
                EnrollmentCount = _queriesServices.CountAllEntities<Enrollment>(),
            };
            return Ok(counts);
        }

        [HttpGet("Faculty-With_Supervisor")]
        public IActionResult FacultyWithSupervisor()
        {
            var result = _queriesServices.AllFacultiesWithOrWithoutSupervisor();
            return Ok(result);
        }

        [HttpGet("Course-With-Assigned-Instructor")]
        public IActionResult CourseWithInstructor(string course)
        {
            var result = _queriesServices.InstructorWithParticularCourse(course);
            return Ok(result);
        }

        [HttpGet("Student-Enrolled-In-Course")]
        public IActionResult CourseWithStudent(string course)
        {
            var result = _queriesServices.StudentEnrolledInParticularCourse(course);
            return Ok(result);
        }

        [HttpGet("Courses-With-Marks")]
        public IActionResult CoursesWithMarks() 
        {
            var result = _queriesServices.CourseWithMarks();
            return Ok(result);
        }

        [HttpGet("Student-With-Highest-Makrs")]
        public IActionResult StudentWithHighestMarks()
        {
            var result = _queriesServices.StudentsWithHighestMarks();
                return Ok(result);
        }

        [HttpGet("Students-With_Marks")]
        public IActionResult StudentsWithMarks()
        {
            var result = _queriesServices.StudentsWithHighestMarks();
            return Ok(result);
        }

        [HttpGet("Students-With-Club-Faculty-Course")]
        public IActionResult StudentsWithCourseWithFacultyAndClub()
        {
            var result = _queriesServices.GetStudentsWithClubsAndCoursesWithFaculty();
            return Ok(result);
        }

        [HttpGet("StudentsWithHighestAndLowestMarks")]
        public IActionResult StudentsWithHighestAndLowestMarks()
        {
            var result = _queriesServices.GetStudentsWithHighestAndLowestMarks();
            return Ok(result);

        }
    }

}
