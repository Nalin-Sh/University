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
    public class StudentController : ControllerBase
    {
        private readonly StudentServices _studentService;
        private readonly IGenericRepository<Student> _studentRepository;

        public StudentController(StudentServices studentService, IGenericRepository<Student> studentRepository)
        {
            _studentService = studentService;
            _studentRepository = studentRepository;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var students = _studentService.GetStudents().OrderBy(c => c.StudentId);
            return Ok(students);
        }
        [HttpPost]
        public ActionResult AddStudent(StudentRequest request)
        {
            try
            {
                var success = _studentService.AddStudent(request);

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
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            bool deleted = _studentService.DeleteStudent(id);
            if (deleted)
            {
                return Ok("Student Deleted Successfully");
            }
            return BadRequest("Deletion Not Successful");
        }
        [HttpPut]
        public IActionResult Update(int id, StudentRequest request)
        {
            bool updated = _studentService.UpdateStudent(id, request);
            if (updated)
            {
                return Ok("Student Updated Successfully");
            }
            return BadRequest("Update Not Successful");

        }
    }
}
