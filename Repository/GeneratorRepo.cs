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
}