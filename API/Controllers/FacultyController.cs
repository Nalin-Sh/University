using Application.Contracts;
using Application.RequestModels;
using Domain.Models;
using Infrastructures.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly FacultyServices _facultyService;
        private readonly IGenericRepository<Faculty> _facultyRepository;

        public FacultyController(FacultyServices facultyService, IGenericRepository<Faculty> facultyRepository)
        {
            _facultyService = facultyService;
            facultyRepository = _facultyRepository;
        }

        [HttpPost]
        public IActionResult AddFaculty([FromBody] FacultyRequest faculty)
        {
            try
            {
                var success = _facultyService.AddFaculty(faculty.Name, faculty.SupervisorID);

                if (success)
                {
                    return Ok("Faculty added successfully");
                }
                else
                {
                    return BadRequest("Failed to add faculty");
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
            var faculty = _facultyService.GetFaculty(); 
            return Ok(faculty);
        }

        [HttpPut]
        public IActionResult Update(int id, FacultyRequest request)
        {
            bool updated = _facultyService.UpdateFaculty(id,request);
            if (updated)
            {
                return Ok("Faculty Updated Successfully");
            }
            return BadRequest("Update Not Successful");
            
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            bool deleted = _facultyService.DeleteFaculty(id);
            if (deleted)
            {
                return Ok("Faculty Deleted Successfully");
            }
            return BadRequest("Deletion Not Successful");
        }
    }
}
