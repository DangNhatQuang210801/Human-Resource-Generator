using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Repository;

public interface IGeneratorRepo
{
    public List<Employee> GetAllEmployeesJoinedAnyTrainingProgram();
    public List<Employee> SearchAllEmployee(string SearchName);
    public  bool CreateEmployee(Employee employee);
}