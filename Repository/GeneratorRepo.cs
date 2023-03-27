using Human_Resource_Generator.Data;
using Human_Resource_Generator.Models;
using Microsoft.EntityFrameworkCore;

namespace Human_Resource_Generator.Repository;

public class GeneratorRepo : IGeneratorRepo
{
    private readonly ApplicationDbContext _applicationDbContext;

    public GeneratorRepo(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public List<Employee> GetAllEmployeesJoinedAnyTrainingProgram()
    {
        var result = _applicationDbContext.Employees.Include(t => t.employee_training).ThenInclude(t=>t.TrainingProgram).ToList();
        // var result = _applicationDbContext.EmployeeTrainings.Include(t => t.Employee)
        //     .Include(t => t.TrainingProgram).ToList();
        return result;
    }
    public List<Employee> SearchAllEmployee(string SearchName)
    {
            var search = _applicationDbContext.Employees
                .Include(t => t.employee_training)
                .ThenInclude(t=>t.TrainingProgram)
                .Where(s => s.employee_name.Contains(SearchName) || s.employee_department.Contains(SearchName)).ToList();
            return search;
    }

    public bool CreateEmployee(Employee employee)
    {
        try
        {
            _applicationDbContext.Employees.Add(employee);
            _applicationDbContext.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}