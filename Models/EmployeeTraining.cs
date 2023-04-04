using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.Models

{
    public class EmployeeTraining
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Required]
        public int TrainingProgramId { get; set; }
        public TrainingProgram TrainingProgram { get; set; }
        public int? Score { get; set; }
    }
}


