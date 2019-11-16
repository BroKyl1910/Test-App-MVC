using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TestApp.MVC.Models
{
    public partial class TestAppContext : DbContext
    {
        public TestAppContext()
        {
        }

        public TestAppContext(DbContextOptions<TestAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answer { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<LecturerAssignment> LecturerAssignment { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<ModuleCourse> ModuleCourse { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<Result> Result { get; set; }
        public virtual DbSet<StudentAssignment> StudentAssignment { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-GA32F87\\SQLEXPRESS;Database=TestApp;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.AnswerId).HasColumnName("AnswerID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.TestId).HasColumnName("TestID");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answer)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Answer__Question__5165187F");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Answer)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Answer__TestID__5070F446");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Answer)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Answer__Username__52593CB8");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.CourseId)
                    .HasColumnName("CourseID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LecturerAssignment>(entity =>
            {
                entity.Property(e => e.LecturerAssignmentId).HasColumnName("LecturerAssignmentID");

                entity.Property(e => e.ModuleId)
                    .IsRequired()
                    .HasColumnName("ModuleID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.LecturerAssignment)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LecturerA__Modul__46E78A0C");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.LecturerAssignment)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LecturerA__Usern__45F365D3");
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.Property(e => e.ModuleId)
                    .HasColumnName("ModuleID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ModuleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ModuleCourse>(entity =>
            {
                entity.Property(e => e.ModuleCourseId).HasColumnName("ModuleCourseID");

                entity.Property(e => e.CourseId)
                    .IsRequired()
                    .HasColumnName("CourseID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ModuleId)
                    .IsRequired()
                    .HasColumnName("ModuleID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.ModuleCourse)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ModuleCou__Cours__3F466844");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.ModuleCourse)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ModuleCou__Modul__3E52440B");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.Answer1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Answer2)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Answer3)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.QuestionText)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TestId).HasColumnName("TestID");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Question__TestID__4D94879B");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.Property(e => e.ResultId).HasColumnName("ResultID");

                entity.Property(e => e.ResultPercentage).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TestId).HasColumnName("TestID");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.TestId)
                    .HasConstraintName("FK__Result__TestID__5535A963");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Result)
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("FK__Result__Username__5629CD9C");

                entity.Property(e => e.ResultDate).HasColumnName("ResultDate");

            });

            modelBuilder.Entity<StudentAssignment>(entity =>
            {
                entity.Property(e => e.StudentAssignmentId).HasColumnName("StudentAssignmentID");

                entity.Property(e => e.CourseId)
                    .IsRequired()
                    .HasColumnName("CourseID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.StudentAssignment)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentAs__Cours__4316F928");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.StudentAssignment)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentAs__Usern__4222D4EF");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.Property(e => e.TestId).HasColumnName("TestID");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.PublishDate).HasColumnType("datetime");

                entity.Property(e => e.Published).HasColumnType("bool");


                entity.Property(e => e.ModuleId)
                    .IsRequired()
                    .HasColumnName("ModuleID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.Test)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Test__ModuleID__4AB81AF0");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Test)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Test__Username__49C3F6B7");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.HasIndex(e => e.UniversityIdentification)
                    .HasName("UQ__User__05F4F44DBD099335")
                    .IsUnique();

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.UniversityIdentification)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });
        }
    }
}
