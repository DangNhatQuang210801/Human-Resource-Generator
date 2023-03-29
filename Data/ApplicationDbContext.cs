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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeTraining>()
                .HasKey(pt => new { pt.employee_id, pt.program_id });

            modelBuilder.Entity<EmployeeTraining>()
                .HasOne(pt => pt.Employee)
                .WithMany(pt => pt.employee_training)
                .HasForeignKey(p => p.employee_id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<EmployeeTraining>()
                .HasOne(pt => pt.TrainingProgram)
                .WithMany(pt => pt.employee_training)
                .HasForeignKey(p => p.program_id)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
        public DbSet<Human_Resource_Generator.ViewModels.TrainingProgramViewModels.TrainingProgramViewModel>? TrainingProgramViewModel { get; set; }
        public DbSet<Human_Resource_Generator.ViewModels.TrainingProgramViewModel.CreateTrainingProgramViewModel>? CreateTrainingProgramViewModel { get; set; }
        public DbSet<Human_Resource_Generator.ViewModels.EmployeeViewModels.EmployeeViewModel>? EmployeeViewModel { get; set; }
        public DbSet<Human_Resource_Generator.ViewModels.EmployeeViewModels.CreateEmployeeViewModel>? CreateEmployeeViewModel { get; set; }

    }
}
