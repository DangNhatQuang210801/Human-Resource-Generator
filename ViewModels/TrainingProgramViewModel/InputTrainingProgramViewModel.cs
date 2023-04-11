using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.ViewModels.TrainingProgramViewModel
{
    public class InputTrainingProgramViewModel: TrainingProgram
    {
        public string? EmployeeIds { get; set; }
        public string? SearchingEmployee { get; set; }
    }
}
