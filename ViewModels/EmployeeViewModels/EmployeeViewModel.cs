using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.ViewModels.EmployeeViewModels
{
    public class EmployeeViewModel
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public DateTime Birthday { get; set; }
    }
}
