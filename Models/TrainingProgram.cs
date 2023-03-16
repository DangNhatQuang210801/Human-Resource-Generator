using System.ComponentModel.DataAnnotations;

namespace Human_Resource_Generator.Models;

public class TrainingProgram
{
    [Required]
    [Key]
    public int program_id { get; set; }
    public string program_name { get; set;}
    public string program_description { get; set; }
    public DateTime date_of_program { get; set; }
    public IEnumerable<EmployeeTraining> employees { get; set; }
}