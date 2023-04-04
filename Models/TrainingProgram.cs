using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.Models;

[Table("TrainingProgram")]
public class TrainingProgram
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set;}
    public string Description { get; set; }
    [Required]
    public string Teacher { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<EmployeeTraining> EmployeeTrainings { get; set; }
}