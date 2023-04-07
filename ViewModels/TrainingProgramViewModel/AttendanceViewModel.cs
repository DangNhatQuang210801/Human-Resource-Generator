using Human_Resource_Generator.Models;
using Human_Resource_Generator.ViewModels.AttendanceViewModels;

namespace Human_Resource_Generator.ViewModels.TrainingProgramViewModel
{
    public class AttendanceViewModel : TrainingProgram
    {
        public List<Employee> JoinedEmployees { get; set; }
        public List<JoinedEmployeeWithDateViewModel> JoinedEmployeeWithDate { get; set; }
    }
}
