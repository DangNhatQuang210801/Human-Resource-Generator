using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.ViewModels.EmployeeViewModels
{
    [Keyless]
    public class CreateEmployeeViewModel
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public DateTime Birthday { get; set; }
        public List<SelectListItem> TrainingPrograms { get; set; }

        public string[] SelectedTrainingProgram { get; set; }
    }
}
