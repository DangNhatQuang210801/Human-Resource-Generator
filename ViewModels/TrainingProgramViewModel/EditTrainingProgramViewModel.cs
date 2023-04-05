using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.ViewModels.TrainingProgramViewModel
{
    public class EditTrainingProgramViewModel: TrainingProgram
    {
        public List<int> JoinedEmployeeIds { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
