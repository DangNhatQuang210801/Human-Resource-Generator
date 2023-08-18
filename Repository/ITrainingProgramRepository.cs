using Human_Resource_Generator.Models;
using Human_Resource_Generator.ViewModels.TrainingProgramViewModel;

namespace Human_Resource_Generator.Repository
{
    public interface ITrainingProgramRepository
    {
        public List<TrainingProgram> GetAll();
        public TrainingProgram? GetById(int programId);
        public Task<int> Add(TrainingProgram trainingProgram);
        public void Update(TrainingProgram trainingProgram);
        public void Delete(TrainingProgram trainingProgram);
        public List<TrainingProgram> GetAllByFilter(string? name);
        public int GetScoreEmployee(int trainingProgramId, int employeeId);
        public List<Employee>GetAllEmployeesByTrainingProgramId (int TrainingProgramId);
    }
}
