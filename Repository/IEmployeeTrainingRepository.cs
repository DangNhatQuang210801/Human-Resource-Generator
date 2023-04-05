using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Repository
{
    public interface IEmployeeTrainingRepository
    {
        public List<EmployeeTraining> GetAll();
        public EmployeeTraining? GetById(int id);
        public void Add(EmployeeTraining employeeTraining);
        public void Update(EmployeeTraining employeeTraining);
        public void Delete(EmployeeTraining employeeTraining);
        public List<EmployeeTraining> GetByTrainingProgramId(int id);
        public EmployeeTraining? GetByTrainingProgramIdAndEmployeeId(int trainingId, int employeeId);
        public void DeleteByTrainingProgramIdAndEmployeeId(int trainingId, int employeeId);
        public void DeleteByTrainingProgramId(int trainingId);
    }
}
