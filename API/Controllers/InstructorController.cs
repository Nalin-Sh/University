using Application.Contracts;
using Application.RequestModels;
using Domain.Models;
using Infrastructures.Data;
using Infrastructures.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Instructor> _instructorRepository;

        public InstructorController(ApplicationDbContext context, IGenericRepository<Instructor> instructorRepository)
        {
            _context = context;
            _instructorRepository = instructorRepository;
        }

        [HttpGet]
        public IActionResult GetAll(int? id, string? name)
        {
            if (id.HasValue)
            {
                var instructor = _instructorRepository.GetAll().FirstOrDefault(c => c.Id == id.Value);

                if (instructor == null)
                {
                    return NotFound("Instructor Not Found");
                }

                return Ok(instructor);
            }

            else if(name != null)
            {
                var instructor = _instructorRepository.GetAll().FirstOrDefault(c => string.Equals(c.Name , name , StringComparison.OrdinalIgnoreCase));
                if (instructor == null)
                {
                    return NotFound("Instructor Not Found");
                }

                return Ok(instructor);

            }

            else
            {
                var instructor = _instructorRepository.GetAll().OrderBy(c => c.Id);
                return Ok(instructor);
            }
        }
        [HttpPost]
        public IActionResult Add(InstructorRequest instructor)
        {
            var addInstructor = new Instructor()
            {
                Name = instructor.FullName,
                Id = (_context.Instructors.ToList().Max(c => c.Id)) + 1

            };
            var instructorAdded = _instructorRepository.Add(addInstructor);
            if (!instructorAdded)
            {
                return BadRequest("Instructor Addition Not Successfull");
            }
            return Ok("Instructor added Successfully");
        }
        [HttpPut]
        public IActionResult Update(int id ,[FromBody] InstructorRequest instructor) 
        {
            var insToUpdate = _instructorRepository.GetAll().FirstOrDefault(i=> i.Id == id);
            if (insToUpdate == null)
            {
                return NotFound("Instructor Not Found");
            }
            insToUpdate.Name = instructor.FullName;
            bool updated = _instructorRepository.Update(insToUpdate);
            if (!updated)
            {
                return BadRequest("Update Not Successful");
            }
            else
            {
                return Ok("Instructor Update Successfully");
            }
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var insToDelete = _instructorRepository.GetAll().FirstOrDefault(i=> i.Id == id);
            if (insToDelete == null)
            {
                return NotFound("Instructor Not Found");
            }
            bool deleted = _instructorRepository.Delete(insToDelete);
            if (!deleted)
            {
                return BadRequest("Deletion Not Successful");
            }
            else 
            {
                return Ok("Instructor Deleted Successfully");
            }

        }
    }
}
