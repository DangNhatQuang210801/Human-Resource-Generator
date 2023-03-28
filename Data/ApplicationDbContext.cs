using Human_Resource_Generator.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Human_Resource_Generator.Data

{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {

        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<TrainingProgram> TrainingProgram { get; set; }  
        public DbSet<EmployeeTraining> EmployeeTraining { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeTraining>()
                .HasKey(bc => new { bc.employee_id, bc.program_id }); 
            
            modelBuilder.Entity<EmployeeTraining>()
                .HasOne(bc => bc.Employee)
                .WithMany(b => b.employee_training)
                .HasForeignKey(bc => bc.program_id);  
            
            
            modelBuilder.Entity<EmployeeTraining>()
                .HasOne(bc => bc.TrainingProgram)
                .WithMany(c => c.employee_training)
                .HasForeignKey(bc => bc.employee_id);
        }

    }
}
