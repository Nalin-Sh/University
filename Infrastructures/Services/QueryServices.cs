using Application.Contracts;
using Domain.Models;
using Infrastructures.Data;
using Infrastructures.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Services
{
    public class QueryServices : IQueriesServices
    {
        private readonly IGenericRepository<Club> _clubRepository;
        private readonly IGenericRepository<Student> _studenRepository;
        private readonly IGenericRepository<Faculty> _facultyRepository;
        private readonly IGenericRepository<Instructor> _instructorRepository;
        private readonly IGenericRepository<Course> _courseRepository;
        private readonly IGenericRepository<Courseassignment> _courseAssignment;
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Enrollment> _enrollmentRepository;

        public QueryServices(IGenericRepository<Club> clubRepository , 
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
            var query = from Club in _clubRepository.GetAll()
                        join Students in _studenRepository.GetAll() on Club.Id equals Students.ClubId into studentClub
                        from sc in studentClub
                        select new
                        {
                            ClubName = Club.Entitle,
                            StudentCount = studentClub.Count(),
                            
                            
                        };
            return query.ToList();
        }

        public IEnumerable<object> AllCoursesInDecending()
        {
            var query = _courseRepository.GetAll();
                return query.ToList().OrderByDescending(x => x.Title);
        }

        public IEnumerable<object> AllCoursesWithNoOfInstructors()
        {
            var distinctCountsQuery = from courseAssignment in _courseAssignment.GetAll()
                                      group courseAssignment by courseAssignment.CourseId into g
                                      select new
                                      {
                                          CourseId = g.Key,
                                          Instructors = g.Select(ic => ic.InstructorId).Distinct().Count()
                                      };

            var query = from course in _courseRepository.GetAll()
                        join distinctCounts in distinctCountsQuery on course.Id equals distinctCounts.CourseId into counts
                        from count in counts.DefaultIfEmpty()
                        select new
                        {
                            Course = course.Title,
                            Instructors = count?.Instructors ?? 0
                        };

            return query.ToList();
        }

        public IEnumerable<object> AllFacultiesWithOrWithoutSupervisor()
        {
            var query = from Faculty in _facultyRepository.GetAll()
                        join Instructor in _instructorRepository.GetAll() on Faculty.SupervisorId equals Instructor.Id into facultyInstructor
                        from fi in facultyInstructor
                        select new
                        {
                            Faculty = Faculty.Name,
                            HasSupervisor = facultyInstructor.Any() ? "Yes" : "NO"

                        };
            return query.ToList();
        }

        public int CountAllEntities<T>() where T : class
        {
            return _context.Set<T>().Count();
        }

        public IEnumerable<object> GetCoursesWithFaculty()
        {
            var query = from Course in _courseRepository.GetAll()
                        join Faculty in _facultyRepository.GetAll() on Course.FacultyId equals Faculty.Id into courseFaculty
                        from cf in courseFaculty
                        select new
                        {
                            Course = Course.Title,
                            Faculty = cf.Name
                        };

            return query.ToList();
         }

        public IEnumerable<object> GetFacultiesWithSupervisor()
        {
            var query = from Faculty in _facultyRepository.GetAll()
                        join Instructor in _instructorRepository.GetAll() on Faculty.SupervisorId equals Instructor.Id into facultySupervisor
                        from s in facultySupervisor
                        select new
                        {
                            Name = Faculty.Name,
                            SupervisorName = s.Name,

                        };

            return query.ToList();
                            
        }

        public IEnumerable<object> InstructorWithParticularCourse(string CourseName)
        {
            var query = from Course in _courseRepository.GetAll()
                        where Course.Title.IndexOf(CourseName, StringComparison.OrdinalIgnoreCase) >= 0
                        join Courseassignment in _courseAssignment.GetAll() on Course.Id equals Courseassignment.CourseId into assignedCourse
                        from assignments in assignedCourse
                        join Instructor in _instructorRepository.GetAll() on assignments.InstructorId equals Instructor.Id into assignedInstructor
                        from instructor in assignedInstructor
                        select new
                        {
                            Course = Course.Title,
                            Instructor = instructor.Name,

                        };
            return query.ToList();
        }

        public IEnumerable<object> StudentEnrolledInParticularCourse(string course)
        {
            var query = from courses in _courseRepository.GetAll()
                        where courses.Title.IndexOf(course, StringComparison.OrdinalIgnoreCase) >= 0
                        join Enrollment in _enrollmentRepository.GetAll() on courses.Id equals Enrollment.CourseId into enrolledCourse
                        from enrolled in enrolledCourse
                        join Student in _studenRepository.GetAll() on enrolled.StudentId equals Student.StudentId into studentEnrolled
                        from student in studentEnrolled
                        select new
                        {
                            Course = courses.Title,
                            Students = student.StudentName
                        };

            return query.ToList();
        }

        public IEnumerable<object> CourseWithMarks()
        {
            var query = from course in _courseRepository.GetAll()
                        join enrollment in _enrollmentRepository.GetAll() on course.Id equals enrollment.CourseId into enrolledCourses
                        from enrolled in enrolledCourses
                        group enrolled by enrolled.CourseId into groupedEnrollments
                        select new
                        {
                            CourseTitle = groupedEnrollments.First().Course.Title,
                            HighestMarks = groupedEnrollments.Max(e => e.Marks),
                            LowestMarks = groupedEnrollments.Min(e => e.Marks),
                            AverageMarks = groupedEnrollments.Average(e => e.Marks),
                        };
            return query.ToList();
        }

        public IEnumerable<object> StudentsWithHighestMarks()
        {
            var query = from course in _courseRepository.GetAll()
                        join enrollment in _enrollmentRepository.GetAll() on course.Id equals enrollment.CourseId into enrolledCourses
                        from enrolled in enrolledCourses
                        join students in _studenRepository.GetAll() on enrolled.StudentId equals students.StudentId into studenEnrolled
                        from student in studenEnrolled
                        let highestmarks = enrolledCourses.Max(e => e.Marks)
                        where enrolled.Marks == highestmarks
                        select new
                        {
                            Course = course.Title,
                            HighestMarks = enrolledCourses.Max(e => e.Marks),
                            StudentName = student.StudentName,
                        };
            return query.ToList();
        }

        public IEnumerable<object> StrudentsWithTheirMarks()
        {
            var query = from students in _studenRepository.GetAll()
                        join enrollment in _enrollmentRepository.GetAll() on students.StudentId equals enrollment.StudentId into studentsEnrolled
                        from enrolled in studentsEnrolled
                        join courses in _courseRepository.GetAll() on enrolled.CourseId equals courses.Id into coursesEnrolled
                        from enrolledStudents in coursesEnrolled
                        select new
                        {
                            StudentName = students.StudentName,
                            Course = enrolled.Course,
                            Marks = enrolled.Marks
                        };

            return query.ToList();


        }

        public IEnumerable<object> GetStudentsWithClubsAndCoursesWithFaculty()
        {
            var query = from students in _studenRepository.GetAll()
                        join enrollment in _enrollmentRepository.GetAll() on students.StudentId equals enrollment.StudentId into studentsEnrolled
                        from enrolled in studentsEnrolled
                        join courses in _courseRepository.GetAll() on enrolled.CourseId equals courses.Id into coursesEnrolled
                        from courseFaculty in coursesEnrolled
                        join faculty in _facultyRepository.GetAll() on courseFaculty.FacultyId equals faculty.Id into facultyEnrolled
                        from faculty in facultyEnrolled
                        join clubs in _clubRepository.GetAll() on students.ClubId equals clubs.Id into studentClubs
                        from club in studentClubs
                        select new
                        {
                            StudentName = students.StudentName,
                            Course = courseFaculty.Title,
                            Faculty = faculty.Name,
                            ClubName = club.Entitle
                        };

        

            return query.ToList();
        }

        public IEnumerable<object> GetStudentsWithHighestAndLowestMarks()
        {
            var query = from students in _studenRepository.GetAll()
                        join enrollment in _enrollmentRepository.GetAll() on students.StudentId equals enrollment.StudentId into studentsEnrolled
                        from enrolled in studentsEnrolled
                        join courses in _courseRepository.GetAll() on enrolled.CourseId equals courses.Id into coursesEnrolled
                        from courseFaculty in coursesEnrolled
                        let highestSubject = _enrollmentRepository.GetAll()
                                                    .Where(e => e.StudentId == students.StudentId)
                                                    .OrderByDescending(e => e.Marks)
                                                    .FirstOrDefault()
                        let lowestSubject = _enrollmentRepository.GetAll()
                                                   .Where(e => e.StudentId == students.StudentId)
                                                   .OrderBy(e => e.Marks)
                                                   .FirstOrDefault()
                        select new
                        {
                            StudentName = students.StudentName,
                            Course = courseFaculty.Title,
                            HighestSubject =  highestSubject.Course.Title,
                            HighestMarks =  highestSubject.Marks,
                            LowestSubject = lowestSubject.Course.Title,
                            LowestMarks =lowestSubject.Marks
                        };
            return query.ToList();
        }
    }
}
