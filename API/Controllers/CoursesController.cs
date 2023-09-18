using Application.Contracts;
using Application.RequestModels;
using Domain.Models;
using Infrastructures.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly CourseServices _courseService;
        private readonly IGenericRepository<Course> _courseRepository;

        public CoursesController(CourseServices courseService, IGenericRepository<Course> courseRepository)
        {
            _courseService = courseService;
            _courseRepository = courseRepository;
        }

        [HttpPost]
        public ActionResult AddCourse(CourseRequest course)
        {
            try
            {
                var success = _courseService.AddCourse(course.Title, course.FacultyId, course.Credits);

                if (success == true)
                {
                    return Ok("Course added successfully");
                }
                else
                {
                    return BadRequest("Course failed to add");
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = _courseService.GetCourses().OrderBy(c => c.CourseId);
            return Ok(courses);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            bool deleted = _courseService.DeleteCourse(id);
            if (deleted)
            {
                return Ok("Course Deleted Successfully");
            }
            return BadRequest("Deletion Not Successful");
        }
        [HttpPut]
        public IActionResult Update(int id, CourseRequest request)
        {
            bool updated = _courseService.UpdateCourse(id, request);
            if (updated)
            {
                return Ok("Course Updated Successfully");
            }
            return BadRequest("Update Not Successful");

        }
    }
}
