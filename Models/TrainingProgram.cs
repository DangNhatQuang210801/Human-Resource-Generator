using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.Models;

public class TrainingProgram
{
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string program_id { get; set; }
    public string program_name { get; set;}
    public string program_description { get; set; }
    public DateTime date_of_program { get; set; }
    public ICollection<EmployeeTraining> EmployeeTrainings { get; set; }
}