using Human_Resource_Generator.Models;
using Microsoft.EntityFrameworkCore;
using Human_Resource_Generator.ViewModels.TrainingProgramViewModels;
using Human_Resource_Generator.ViewModels.TrainingProgramViewModel;
using Human_Resource_Generator.ViewModels.EmployeeViewModels;

namespace Human_Resource_Generator.Data

{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<EmployeeTraining> EmployeeTrainings { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                .HasOne(et => et.EmployeeTraining);
        }
    }
}
