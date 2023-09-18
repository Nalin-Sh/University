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
    public class CourseAssignmentController : ControllerBase
    {
        private readonly CourseAssignmentServices _courseAssignmentService;
        private readonly IGenericRepository<Courseassignment> _courseAssignmentRepository;

        public CourseAssignmentController(CourseAssignmentServices courseAssignmentService, IGenericRepository<Courseassignment> courseAssignmentRepository)
        {
            _courseAssignmentService = courseAssignmentService;
            _courseAssignmentRepository = courseAssignmentRepository;
        }

        [HttpPost]
        public IActionResult AddAssignment(CourseAssignmentRequest request)
        {
            try
            {
                var success = _courseAssignmentService.AssignCourseToInstructor(request);

                if (success)
                {
                    return Ok("Course assigned successfully");
                }
                else
                {
                    return BadRequest("Course assignment failed");
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpGet]
        public IActionResult GetInstructors()
        {
            var instructorWithCourses = _courseAssignmentService.GetAll().OrderBy(c => c.InstructorId);
            return Ok(instructorWithCourses);
        }
        [HttpPut]
        public IActionResult Update(CourseAssignmentRequest request)
        {
            bool updated = _courseAssignmentService.UpdateCourseAssignment(request);
            if (updated)
            {
                return Ok("Course Assignment Updated Successfully");
            }
            return BadRequest("Update Not Successful");

        }
        [HttpDelete]
        public IActionResult Delete(CourseAssignmentRequest request)
        {
            bool updated = _courseAssignmentService.DeleteCourseAssignment(request);
            if (updated)
            {
                return Ok("Course Assignment Deleted Successfully");
            }
            return BadRequest("Deletion Not Successful");

        }

    }
}
