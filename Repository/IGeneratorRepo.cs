using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Repository;

public interface IGeneratorRepo
{
    public List<EmployeeTraining> GetAllEmployeesJoinedAnyTrainingProgram();
    public List<EmployeeTraining> SearchAllEmployee(string SearchName);
}