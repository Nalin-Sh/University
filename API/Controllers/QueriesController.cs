using Application.Contracts;
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
    }

}
