using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.Models;

public class Employee
{
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public DateTime Birthday { get; set; }
        public List<EmployeeTraining> EmployeeTrainings { get; set; } = new List<EmployeeTraining>();
}