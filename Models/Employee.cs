using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.Models;

public class Employee
{
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int employee_id { get; set; }
        public string employee_number { get; set; }
        public string employee_name { get; set; }
        public string employee_department { get; set; }
        public DateTime date_of_birth { get; set; }
        public ICollection<EmployeeTraining> employee_training { get; set; }
}