using System.ComponentModel.DataAnnotations;
namespace Human_Resource_Generator.Models

{
    public class EmployeeTraining
    {
        public int employee_id { get; set; }
        public Employee Employee { get; set; }
        public int program_id { get; set; }
        public TrainingProgram TrainingProgram { get; set; }
    }
}


