using Human_Resource_Generator.Models;
using Microsoft.EntityFrameworkCore;

namespace Human_Resource_Generator.Data

{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee_Training>()
                .HasOne(b => b.employee)
                .WithMany(ba => ba.employees_Training)
                .HasForeignKey(b => b.employee_id);
            modelBuilder.Entity<Employee_Training>()
                .HasOne(b => b.training_Program)
                .WithMany(ba => ba.employees_Training)
                .HasForeignKey(b => b.program_id);
        }
        public DbSet<Employee> employee { get; set; }
        public DbSet<Training_program> training_Program { get; set; }
        public DbSet<Employee_Training> employees_Training { get; set; }
    }
}
