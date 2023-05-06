using Human_Resource_Generator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Human_Resource_Generator.Data

{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<EmployeeTraining> EmployeeTrainings { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceEmployee> AttendanceEmployees { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EmployeeTraining>()
                .HasOne(pt => pt.Employee)
                .WithMany(pt => pt.EmployeeTrainings)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeTraining>()
                .HasOne(pt => pt.TrainingProgram)
                .WithMany(pt => pt.EmployeeTrainings)
                .HasForeignKey(p => p.TrainingProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attendance>()
                .HasOne(et => et.TrainingProgram)
                .WithMany(pt => pt.Attendances)
                .HasForeignKey(p => p.TrainingProgramId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AttendanceEmployee>()
                .HasOne(et => et.Attendance)
                .WithMany(pt => pt.AttendanceEmployees)
                .HasForeignKey(p => p.AttendanceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AttendanceEmployee>()
                .HasOne(et => et.Employee)
                .WithMany(pt => pt.AttendanceEmployees)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
