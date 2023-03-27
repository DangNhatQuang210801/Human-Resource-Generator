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

    public List<EmployeeTraining> GetAllEmployeesJoinedAnyTrainingProgram()
    {

        var result = _applicationDbContext.EmployeeTraining.Include(t => t.Employee).Include(t => t.TrainingProgram).ToList();
        return result;
    }
    public List<EmployeeTraining> SearchAllEmployee(string SearchName)
    {
            var search = _applicationDbContext.EmployeeTraining
            .Include(t => t.Employee)
            .Include(t => t.TrainingProgram)
            .Where(s => s.Employee.employee_name.Contains(SearchName) 
            || s.Employee.employee_id.ToString().Contains(SearchName)
            || s.Employee.employee_department.Contains(SearchName)
            || s.TrainingProgram.program_name.Contains(SearchName)
            || s.TrainingProgram.program_description.Contains(SearchName))
            .ToList();
            return search;
    }
}