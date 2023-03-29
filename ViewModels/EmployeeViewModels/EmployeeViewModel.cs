using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.ViewModels.EmployeeViewModels
{
    public class EmployeeViewModel
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string employee_id { get; set; }
        public string employee_number { get; set; }
        public string employee_name { get; set; }
        public string employee_department { get; set; }
        public DateTime date_of_birth { get; set; }
    }
}
