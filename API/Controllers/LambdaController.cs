using Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LambdaController : ControllerBase
    {
        private readonly ILambdaServices _lambdaServices;

        public LambdaController(ILambdaServices lambdaServices) 
        {
            _lambdaServices = lambdaServices;
        }

        [HttpGet]
        public IActionResult GetClubsWithStudentCount() 
        {
            var result = _lambdaServices.AllClubsWithNoOfStudents();
            return Ok(result);
        }

        [HttpGet("Course-Decending")]
        public IActionResult CoursesDecending()
        {
            var result = _lambdaServices.AllCoursesInDecending();
            return Ok(result);
        }

        [HttpGet("Course-Instructor-Number")]
        public IActionResult CoursesWithInstructorNumber()
        {
            var result = _lambdaServices.AllCoursesWithNoOfInstructors();
            return Ok(result);
        }

        [HttpGet("Faculty-Has-Supervisor")]
        public IActionResult FacultyHasSupervisor()
        {
            var result = _lambdaServices.AllFacultiesWithOrWithoutSupervisor();
            return Ok(result);
        }

        [HttpGet("Course-With-Faculty")]
        public IActionResult CourseWithFaculty()
        {
            var result = _lambdaServices.GetCoursesWithFaculty();
            return Ok(result);
        }

        [HttpGet("Faculty-With-Supervisor")]
        public IActionResult FacultyWithSupervisor()
        {
            var result = _lambdaServices.GetFacultiesWithSupervisor();
            return Ok(result);
        }

        [HttpGet("Course-With-Marks")]
        public IActionResult CourseWithMarks()
        {
            var result = _lambdaServices.CourseWithMarks();
            return Ok(result);
        }

        [HttpGet("Students-With-Marks")]
        public IActionResult StudentsWithMarks()
        {
            var result = _lambdaServices.StrudentsWithTheirMarks();
            return Ok(result);
        }
        [HttpGet("Students-With-Highest-Marks")]
        public IActionResult StudentsWithHighestMarks()
        {
            var result = _lambdaServices.StudentsWithHighestMarks();
            return Ok(result);
        }

        [HttpGet("Students-With-Particular-Course")]
        public IActionResult StudentsWithParticularCourse(string course)
        {
            var result = _lambdaServices.StudentEnrolledInParticularCourse(course);
            return Ok(result);
        }

        [HttpGet("Instructor-With-Particular-Course")]
        public IActionResult InstructorWithParticularCourse(string course)
        {
            var result = _lambdaServices.InstructorWithParticularCourse(course);
            return Ok(result);
        }


    }
}
