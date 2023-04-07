using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Repository
{
    public interface IEmployeeTrainingRepository
    {
        public void Add(EmployeeTraining employeeTraining);
        public void Update(EmployeeTraining employeeTraining);
        public void Delete(EmployeeTraining employeeTraining);
        public List<EmployeeTraining> GetByTrainingProgramId(int id);
        public EmployeeTraining? GetByTrainingProgramIdAndEmployeeId(int trainingId, int employeeId);
        public void DeleteByTrainingProgramIdAndEmployeeId(int trainingId, int employeeId);
        public void DeleteByTrainingProgramId(int trainingId);
        public IEnumerable<Employee> GetAll();
        public Employee GetById(int id);
        public void Insert(Employee employee);
        public void Update(Employee employee);
        public void Delete(int id);
        public List<Employee> GetListDataByListId(List<int> listId);
        public List<Employee> GetEmployeesByName(string name);
    }
}
