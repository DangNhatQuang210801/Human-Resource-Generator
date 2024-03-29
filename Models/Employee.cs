using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.Models;

public class Employee
{
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Position { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string Status { get; set; }
    public string? Description { get; set; }
    public ICollection<EmployeeTraining> EmployeeTrainings { get; set; } = new List<EmployeeTraining>();
    public ICollection<AttendanceEmployee> AttendanceEmployees { get; set; } = new List<AttendanceEmployee>();
}