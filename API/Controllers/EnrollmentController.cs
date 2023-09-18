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
    public class EnrollmentController : ControllerBase
    {
        private readonly EnrollmentServices _enrollmentService;
        private readonly IGenericRepository<Enrollment> _enrollmentRepository;

        public EnrollmentController(EnrollmentServices enrollmentService, IGenericRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentService = enrollmentService;
            _enrollmentRepository = enrollmentRepository;
        }
        [HttpGet]
        public IActionResult GetEnrollment()
        {
            var enrollment = _enrollmentService.GetAll().OrderBy(c => c.EnrollmentId);
            return Ok(enrollment);
        }
        [HttpPut]
        public IActionResult Update(EnrollmentRequest request)
        {
            bool updated = _enrollmentService.UpdateEnrollment(request);
            if (updated)
            {
                return Ok("Enrollment Updated Successfully");
            }
            return BadRequest("Update Not Successful");

        }
        [HttpDelete]
        public IActionResult Delete(EnrollmentRequest request)
        {
            bool updated = _enrollmentService.DeleteEnrollment(request);
            if (updated)
            {
                return Ok("Enrollment Deleted Successfully");
            }
            return BadRequest("Deletion Not Successful");

        }
    }
}
