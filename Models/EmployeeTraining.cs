using System.ComponentModel.DataAnnotations;
namespace Human_Resource_Generator.Models

{
    public class EmployeeTraining
    {
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string ProgramId { get; set; }
        public TrainingProgram TrainingProgram { get; set; }
    }
}


