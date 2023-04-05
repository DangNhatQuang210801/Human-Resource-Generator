using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.ViewModels.TrainingProgramViewModel
{
    public class DetailTrainingProgramViewModel:TrainingProgram
    {
        public List<Employee> Employees { get; set; }
    }
}
