using Application.Contracts;
using Application.RequestModels;
using Domain.Models;
using Infrastructures.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Club> _clubRepository;

        public ClubsController(ApplicationDbContext context, IGenericRepository<Club> clubRepository)
        {
            _context = context;
            _clubRepository = clubRepository;
        }

        [HttpGet]
        public IActionResult GetAll(int? id , string? name)
        {
            if (id.HasValue)
            {
                var club = _clubRepository.GetAll().FirstOrDefault(c => c.Id == id.Value);

                if (club == null)
                {
                    return NotFound("Club Not Found");
                }

                return Ok(club);
            }
            else if (name != null)
            {
                var club = _clubRepository.GetAll().Where(c => c.Entitle.ToLower().Contains(name.ToLower()));
                
                if (club == null)
                {
                    return NotFound("Club Not Found");
                }

                return Ok(club);

            }
            else
            {
                var clubs = _clubRepository.GetAll().OrderBy(c => c.Id);
                return Ok(clubs);
            }
        }


        [HttpPost]
        public IActionResult Add(ClubRequest club) 
        {
            var addClub = new Club()
            {
                Entitle = club.Entitle
            };   
            var clubAdded = _clubRepository.Add(addClub);
            if (!clubAdded)
            {
                return BadRequest("Club Addition Not Successfull");
            }
            return Ok("Club added Successfully");
        }

        [HttpDelete("Id")]
        public IActionResult Delete(int id)
        {
            var clubToDelete = _clubRepository.GetAll().FirstOrDefault(c => c.Id == id);

            if (clubToDelete == null)
            {
                return NotFound("Club Not Found");
            }
            bool deleted = _clubRepository.Delete(clubToDelete);
            if (!deleted)
            {
                return BadRequest("Deletion Not Successful");
            }
            else
            {
                return Ok("Club Deleted Successfully");
            }
        }

        [HttpPut("Id")]
        public IActionResult Update(int id ,[FromBody] ClubRequest name)
        {
            var clubToUpdate = _clubRepository.GetAll().FirstOrDefault(c => c.Id == id);

            if (clubToUpdate == null)
            {
                return NotFound("Club Not Found");
            }
            clubToUpdate.Entitle = name.Entitle;
            bool updated = _clubRepository.Update(clubToUpdate);
            if (!updated)
            {
                return BadRequest("Update Not Successful");
            }
            else
            {
                return Ok("Club Update Successfully");
            }
        }





    }
}
