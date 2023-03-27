using System.ComponentModel.DataAnnotations;

namespace Human_Resource_Generator.Models;

public class Employee
{
        [Required]
        [Key]
        public int employee_id { get; set; }
        public string employee_name { get; set; }
        public string employee_department { get; set; }
        public DateTime date_of_birth { get; set; }
        public IEnumerable<EmployeeTraining> employee_training { get; set; }
}