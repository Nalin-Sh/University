using System;
using System.Collections.Generic;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructures.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Club> Clubs { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Courseassignment> Courseassignments { get; set; } = null!;
        public virtual DbSet<Enrollment> Enrollments { get; set; } = null!;
        public virtual DbSet<Faculty> Faculties { get; set; } = null!;
        public virtual DbSet<Instructor> Instructors { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("User ID=postgres;Password=Kathmandu@123;Host=localhost;Port=5432;Database=University;Pooling=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>(entity =>
            {
                entity.ToTable("clubs");

                entity.Property(e => e.Id).HasColumnName("id")
                                          .UseIdentityColumn();
                entity.Property(e => e.Entitle).HasColumnName("entitle");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("courses");

                entity.Property(e => e.Id).HasColumnName("id")
                                          .UseIdentityColumn(); 

                entity.Property(e => e.Credits).HasColumnName("credits");

                entity.Property(e => e.FacultyId).HasColumnName("faculty_id");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.FacultyId)
                    .HasConstraintName("courses_faculty_id_fkey");
            });

            modelBuilder.Entity<Courseassignment>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("courseassignments");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.InstructorId).HasColumnName("instructor_id");

                entity.HasOne(d => d.Course)
                    .WithMany()
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("courseassignments_course_id_fkey");

                entity.HasOne(d => d.Instructor)
                    .WithMany()
                    .HasForeignKey(d => d.InstructorId)
                    .HasConstraintName("courseassignments_instructor_id_fkey");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("enrollments");

                entity.Property(e => e.Id).HasColumnName("id")
                                          .UseIdentityColumn();

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.Marks).HasColumnName("marks");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("enrollments_course_id_fkey");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("enrollments_student_id_fkey");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("faculty");

                entity.Property(e => e.Id).HasColumnName("id")
                                          .UseIdentityColumn(); 

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.SupervisorId).HasColumnName("supervisor_id");

                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.Faculties)
                    .HasForeignKey(d => d.SupervisorId)
                    .HasConstraintName("faculty_supervisor_id_fkey");
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.ToTable("instructor");

                entity.Property(e => e.Id).HasColumnName("id")
                                          .UseIdentityColumn();

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("students");

                entity.Property(e => e.StudentId).HasColumnName("student_id").UseIdentityColumn(); 

                entity.Property(e => e.ClubId).HasColumnName("club_id");

                entity.Property(e => e.EnrollmentDate).HasColumnName("enrollment_date");

                entity.Property(e => e.StudentName).HasColumnName("student_name");

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.ClubId)
                    .HasConstraintName("students_club_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
