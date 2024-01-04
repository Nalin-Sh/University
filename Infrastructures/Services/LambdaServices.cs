using Application.Contracts;
using Domain.Models;
using Infrastructures.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Services
{
    public class LambdaServices : ILambdaServices
    {

        private readonly IGenericRepository<Club> _clubRepository;
        private readonly IGenericRepository<Student> _studenRepository;
        private readonly IGenericRepository<Faculty> _facultyRepository;
        private readonly IGenericRepository<Instructor> _instructorRepository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IGenericRepository<Courseassignment> _courseAssignment;
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Enrollment> _enrollmentRepository;

        public LambdaServices(IGenericRepository<Club> clubRepository,
            IGenericRepository<Student> studenRepository,
            IGenericRepository<Faculty> facultyRepository,
            IGenericRepository<Instructor> instructorRepository,
            IGenericRepository<Course> courseRepository,
            IGenericRepository<Courseassignment> courseAssignment,
            ApplicationDbContext context,
            IGenericRepository<Enrollment> enrollmentRepository)
        {
            _clubRepository = clubRepository;
            _studenRepository = studenRepository;
            _facultyRepository = facultyRepository;
            _instructorRepository = instructorRepository;
            _courseRepository = courseRepository;
            _courseAssignment = courseAssignment;
            _context = context;
            _enrollmentRepository = enrollmentRepository;
        }

        public IEnumerable<object> AllClubsWithNoOfStudents()
        {
            var query = _clubRepository.GetAll()
                        .GroupJoin(_studenRepository.GetAll(), 
                        club => club.Id,
                        student => student.ClubId, 
                        (club, student) => new 
                        {
                            ClubName = club.Entitle,
                            StudentCount = student.Count()
                        });

            return query.ToList();
        }

        public IEnumerable<object> AllCoursesInDecending()
        {
            var query = _courseRepository.GetAll();
            return query.ToList().OrderByDescending(x => x.Title);
        }

        public IEnumerable<object> AllCoursesWithNoOfInstructors()
        {
            var query = _courseRepository.GetAll()
                        .GroupJoin(_courseAssignment.GetAll(),
                        course => course.Id,
                        assignedCourse => assignedCourse.CourseId,
                        (course, assignedcourse) => new
                        {
                            Course = course.Title,
                            InstructorAssigned = assignedcourse.Count(),

                        });
            return query.ToList();

        }

        public IEnumerable<object> AllFacultiesWithOrWithoutSupervisor()
        {
            var query = _facultyRepository.GetAll()
                        .GroupJoin(_instructorRepository.GetAll(),
                        faculty => faculty.SupervisorId,
                        supervisor => supervisor.Id,
                        (faculty, supervisor) => new
                        {
                            Faculty = faculty.Name,
                            hasSupervisor = supervisor.Any()? "Yes" : "No"

                        });

            return query.ToList();  
                        
        }

        public int CountAllEntities<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> CourseWithMarks()
        {
            var query = _courseRepository.GetAll()
                        .GroupJoin(_enrollmentRepository.GetAll(),
                        course => course.Id,
                        enrolled => enrolled.CourseId,
                        (course, enrolled) => new
                        {
                            courseTitle = course.Title,
                            HighestMarks = enrolled.Max(hm => (int?)hm.Marks),
                            LowestMarks = enrolled.Min(lm => (int?)lm.Marks),
                            AverageMarks = enrolled.Average(am => (double?)am.Marks) 
                        });
            return query.ToList();
        }

        public IEnumerable<object> GetCoursesWithFaculty()
        {
            var query = _courseRepository.GetAll()
                        .GroupJoin(_facultyRepository.GetAll(),
                         course => course.FacultyId,
                         faculty => faculty.Id,
                         (course, faculty) => new
                         {
                             CourseName = course.Title,
                             FacultyName = faculty.FirstOrDefault()?.Name
                         }).ToList();
            return query;
        }

        public IEnumerable<object> GetFacultiesWithSupervisor()
        {
            var query = _facultyRepository.GetAll()
                        .GroupJoin(_instructorRepository.GetAll(),
                        faculty => faculty.SupervisorId,
                        instructor => instructor.Id,
                        (faculty, instructor) => new
                        {
                            Faculty = faculty.Name,
                            Supervisor = instructor.FirstOrDefault()?.Name

                        }).ToList();

            return query;

        }

        public IEnumerable<object> GetStudentsWithClubsAndCoursesWithFaculty()
        {
            var query = _studenRepository.GetAll()
                        .Join(_enrollmentRepository.GetAll(),
                        student => student.StudentId,
                        enrolled => enrolled.StudentId,
                        (student, enrolled) => new
                        {
                            student = student.StudentId,
                            enrolled = enrolled

                        })
                        .Join(_courseRepository.GetAll(),
                        enrolledCourse => enrolledCourse.enrolled.CourseId,
                        course => course.Id,
                        (enrolledCourse, course) => new
                        {
                            enrolledCourse = enrolledCourse,
                            course = course.Id
                        })
                        .Join(_facultyRepository.GetAll(),
                        courseFaculty => courseFaculty.course,
                        faculty => faculty.Id,
                        (courseFaculty, faculty) => new
                        {
                            courseFaculty = courseFaculty,
                            faculty = faculty.Id

                        })
                        .Join(_clubRepository.GetAll(),
                        StudentClub => StudentClub.courseFaculty.enrolledCourse.enrolled.Student.ClubId,
                        club => club.Id,
                        (StudentClub, club) => new
                        {
                            student = StudentClub.courseFaculty.enrolledCourse.enrolled.Student.StudentName,
                            enrollment = StudentClub.courseFaculty.enrolledCourse.enrolled,
                            course = StudentClub.courseFaculty.course,
                            faculty = StudentClub.courseFaculty.enrolledCourse.enrolled.Course.Faculty,
                            StudentClub = StudentClub,
                            club = club.Id
                        }
                        );
                        
                        

            return query.ToList();
                        
        }

        public IEnumerable<object> GetStudentsWithHighestAndLowestMarks()
        {
            var query = _studenRepository.GetAll()
                        .GroupJoin(_enrollmentRepository.GetAll(),
                        student => student.StudentId,
                        enrollment => enrollment.StudentId,
                        (student, enrollment) => new
                        {
                            student = student.StudentName,
                            enrollment = enrollment
                        })
                        .GroupJoin(_courseRepository.GetAll(),
                        enrolledCourse => enrolledCourse.enrollment.FirstOrDefault()?.CourseId,
                        course => course.Id,
                        (course, enrolledCourse) => new
                        {
                            course = course,
                            enrolledCourse = enrolledCourse
                        }).GroupBy(result => result.course.student);
                        
                        
                        
                        
                        return query.ToList();
        }

        public IEnumerable<object> InstructorWithParticularCourse(string CourseName)
        {
            var query = _courseRepository.GetAll()
                       .Where(courses => courses.Title.IndexOf(CourseName, StringComparison.OrdinalIgnoreCase) >= 0)
                       .Join(_courseAssignment.GetAll(),
                       courses => courses.Id,
                       enrollment => enrollment.CourseId,
                       (courses, enrollment) => new
                       {
                           CourseName = courses.Title,
                           enrolled = enrollment
                       })
                       .Join(_instructorRepository.GetAll(),
                       enrollments => enrollments.enrolled.InstructorId,
                       instructor => instructor.Id,
                       (enrollments, instructor) => new
                       {
                           Instructor = instructor.Name,
                       });

            return query.ToList();
        }

        public IEnumerable<object> StrudentsWithTheirMarks()
        {
            var query = _studenRepository.GetAll()
                        .Join(_enrollmentRepository.GetAll(),
                        student => student.StudentId,
                        enrolled => enrolled.StudentId,
                        (student, enrolled) => new
                        {
                            StudentName = student.StudentName,
                            courseId = enrolled.CourseId,
                            enrollement = enrolled
                        })
                        .Join(_courseRepository.GetAll(),
                        enrollment => enrollment.courseId,
                        course => course.Id,
                        (enrollment , course) => new
                        {
                            StudentName = enrollment.StudentName,
                            Course = course.Title,
                            Marks = enrollment.enrollement.Marks,
                        }

                        );
            return query.ToList();

        }

        public IEnumerable<object> StudentEnrolledInParticularCourse(string course)
        {
            var query = _courseRepository.GetAll()
                        .Where(courses => courses.Title.IndexOf(course, StringComparison.OrdinalIgnoreCase) >= 0)
                        .Join(_enrollmentRepository.GetAll(),
                        courses => courses.Id,
                        enrollment => enrollment.CourseId,
                        (courses, enrollment) => new
                        {
                            CourseName = courses.Title,
                            enrolled = enrollment
                        })
                        .Join(_studenRepository.GetAll(),
                        enrollments => enrollments.enrolled.StudentId,
                        student => student.StudentId,
                        (enrollments, student) => new
                        {
                            StudentName = student.StudentName,
                        });

            return query.ToList();         
        }

        public IEnumerable<object> StudentsWithHighestMarks()
        {
            var query = _courseRepository.GetAll()
                        .GroupJoin(_enrollmentRepository.GetAll(),
                        course => course.Id,
                        enrolled => enrolled.CourseId,
                        (course, enrolled) => new
                        {
                            CourseName = course.Title,
                            HighestMarks = enrolled.OrderByDescending(m => m.Marks)?.FirstOrDefault()
                        })
                        .Join(_studenRepository.GetAll(),
                        HighestMarks => HighestMarks.HighestMarks.StudentId,
                        student => student.StudentId,
                        (HighestMarks, student) => new
                        {
                            StudentName = student.StudentName,
                            Course = HighestMarks.CourseName,
                            HighestMarks = HighestMarks.HighestMarks
                        }).ToList();

            return query.ToList();               
        }
    }
}
