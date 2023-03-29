using Human_Resource_Generator.ViewModels.TrainingProgramViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.ViewModels.EmployeeViewModels
{
    [Keyless]
    [NotMapped]
    public class CreateEmployeeViewModel
    {
        public string employee_number { get; set; }
        public string employee_name { get; set; }
        public string employee_department { get; set; }
        public DateTime date_of_birth { get; set; }
        public List<SelectListItem> TrainingPrograms { get; set; }
        public string[] SelectedTrainingProgram { get; set; }
    }
}
