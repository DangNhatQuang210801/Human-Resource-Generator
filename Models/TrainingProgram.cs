using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.Models;

public class TrainingProgram
{
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string Name { get; set;}
    public string Description { get; set; }
    [Required]
    public DateTime Time { get; set; }
    [Required]
    public string Teacher { get; set; }
    public ICollection<EmployeeTraining> EmployeeTrainings { get; set; }
}