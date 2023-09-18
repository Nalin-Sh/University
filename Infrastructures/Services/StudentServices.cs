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
    public class StudentServices
    {
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Club> _clubRepository;
        private readonly ApplicationDbContext _context;

        public StudentServices(IGenericRepository<Student> studentRepository, IGenericRepository<Club> clubRepository, ApplicationDbContext context)
        {
            _studentRepository = studentRepository;
            _clubRepository = clubRepository;
            _context = context;
        }
        public bool AddStudent(StudentRequest request)
        {
            var club = _clubRepository.GetAll().FirstOrDefault(i => i.Id == request.ClubID);
            if (club == null)
            {
                throw new InvalidOperationException("Club not found");
            }
            else
            {
                var newStudent = new Student()
                {
                    StudentName = request.Name,
                    EnrollmentDate = new DateOnly(request.EnrolledDate.Year, request.EnrolledDate.Month, request.EnrolledDate.Day),
                ClubId = request.ClubID,
                    StudentId = (_context.Students.ToList().Max(x => x.StudentId)) + 1
                };
                var result = _studentRepository.Add(newStudent);
                return result;
            }
        }
        public IEnumerable<StudentResponse> GetStudents()
        {
            var student = _studentRepository.GetAll();
            var club = _clubRepository.GetAll();

            var result = from Student in student
                         join Club in club
                         on Student.ClubId equals Club.Id into StudentClub
                         from studentClub in StudentClub.DefaultIfEmpty()
                         select new StudentResponse
                         {
                             StudentId = Student.StudentId,
                             StudentName = Student.StudentName,
                             EnrolledDate = Student.EnrollmentDate,
                             ClubId = studentClub.Id,
                             ClubName = studentClub.Entitle
                         };
            return result.ToList();
        }
        public bool DeleteStudent(int id)
        {
            var studentToDelete = _context.Students.FirstOrDefault(f => f.StudentId == id);
            bool deleted = _studentRepository.Delete(studentToDelete);
            if (deleted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateStudent(int id, StudentRequest request)
        {

            var student = _context.Students.FirstOrDefault(f => f.StudentId == id);
            if (student == null)
            {
                throw new InvalidOperationException("Student not found");
            }
            student.StudentName = request.Name;
            student.ClubId = request.ClubID;
            student.EnrollmentDate = new DateOnly(request.EnrolledDate.Year, request.EnrolledDate.Month, request.EnrolledDate.Day);
            _context.SaveChanges();
            return true;
        }


    }
}
