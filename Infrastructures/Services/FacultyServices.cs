using Application.Contracts;
using Application.RequestModels;
using Application.ResponseModels;
using Domain.Models;
using Infrastructures.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Services
{
    public class FacultyServices 
    {
        private readonly IGenericRepository<Faculty> _facultyRepository;
        private readonly IGenericRepository<Instructor> _instructorRepository;
        private readonly ApplicationDbContext _context;

        public FacultyServices(IGenericRepository<Faculty> facultyRepository, IGenericRepository<Instructor> instructorRepository, ApplicationDbContext context)
        {
            _facultyRepository = facultyRepository;
            _instructorRepository = instructorRepository;
            _context = context;
        }

        public IGenericRepository<Faculty> GetFaculties()
        {
            return _facultyRepository;
        }

        public bool AddFaculty(string facultyName, int instructorId) 
        {
            var instructor = _instructorRepository.GetAll().FirstOrDefault(i=> i.Id == instructorId);
            if (instructor == null)
            {
                throw new InvalidOperationException("Instructor not found");
            }

            var newFaculty = new Faculty()
            {
                Name = facultyName,
                SupervisorId = instructorId,
                Id = (_context.Faculties.ToList().Max(x=> x.Id)) + 1
            };
            var result = _facultyRepository.Add(newFaculty);
            return result;

        }

        public IEnumerable<FacultyResponse> GetFaculty()
        {
            var faculty = _facultyRepository.GetAll();
            var instructor = _instructorRepository.GetAll();

            var result = from Faculty in faculty
                         join Instructor in instructor
                         on Faculty.SupervisorId equals Instructor.Id into SupervisorGroup
                         from supervisor in SupervisorGroup.DefaultIfEmpty()
                         select new FacultyResponse
                         { 
                             FacultyId = Faculty.Id,
                             FacultyName =Faculty.Name, 
                             SupervisorID = supervisor.Id,
                            SupervisorName =supervisor.Name

                         };
            return result.ToList();
        }
        public bool UpdateFaculty(int id,FacultyRequest request)
        {

            var faculty = _context.Faculties.FirstOrDefault(f=> f.Id == id);
            if (faculty == null)
            {
                throw new InvalidOperationException("Faculty not found");
            }
            faculty.Name = request.Name;
            faculty.SupervisorId = request.SupervisorID;
            _context.SaveChanges();
            return true;
        }
        public bool DeleteFaculty(int id)
        {
            var facultyToDelete = _context.Faculties.FirstOrDefault(f=> f.Id == id);
            bool deleted = _facultyRepository.Delete(facultyToDelete);
            if (deleted)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
    }
}
