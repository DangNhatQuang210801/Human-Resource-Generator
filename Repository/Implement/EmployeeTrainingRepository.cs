using Human_Resource_Generator.Data;
using Human_Resource_Generator.Models;
using Microsoft.EntityFrameworkCore;

namespace Human_Resource_Generator.Repository.Implement
{
    public class EmployeeTrainingRepository:IEmployeeTrainingRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeTrainingRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Delete(EmployeeTraining employeeTraining)
        {
            _db.EmployeeTrainings.Remove(employeeTraining);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var employee = _db.Employees.FirstOrDefault(x => x.Id == id);
            _db.Employees.Remove(employee);
            _db.SaveChanges();
        }

        public IEnumerable<Employee> GetAll()
        {
            return _db.Employees.ToList();
        }

        public Employee GetById(int id)
        {
            var employees = _db.Employees.Include(e => e.EmployeeTrainings).FirstOrDefault(x => x.Id == id) ?? new Employee();;
            var employeeTrainings = employees.EmployeeTrainings.Select(et => new EmployeeTraining
            {
                TrainingProgram = _db.TrainingPrograms.FirstOrDefault(t => t.Id == et.TrainingProgramId) ??
                                  new TrainingProgram(),
                Id = et.Id,
                EmployeeId = et.EmployeeId
            }).ToList();
            employees.EmployeeTrainings = employeeTrainings;
            return employees;
        }

        public void Insert(Employee employee)
        {
            _db.Employees.Add(employee);
            _db.SaveChanges();
        }

        public void Update(Employee employee)
        {
            _db.Employees.Update(employee);
            _db.SaveChanges();
        }

        public List<Employee> GetListDataByListId(List<int> listId)
        {
            return _db.Employees.Where(e => listId.Contains(e.Id)).ToList();
        }

        public List<Employee> GetEmployeesByName(string name)
        {
            return _db.Employees.Where(e => e.Name.Contains(name)).ToList();
        }

        public void Add(EmployeeTraining employeeTraining)
        {
            _db.EmployeeTrainings.Add(employeeTraining);
            _db.SaveChanges();
        }

        public void Update(EmployeeTraining employeeTraining)
        {
            _db.EmployeeTrainings.Update(employeeTraining);
            _db.SaveChanges();
        }

        public List<EmployeeTraining> GetByTrainingProgramId(int id)
        {
            return _db.EmployeeTrainings.Where(x => x.TrainingProgramId == id).Distinct().ToList();
        }

        public EmployeeTraining? GetByTrainingProgramIdAndEmployeeId(int trainingId, int employeeId)
        {
            return _db.EmployeeTrainings.FirstOrDefault(x => x.TrainingProgramId == trainingId && x.EmployeeId == employeeId);
        }

        public void DeleteByTrainingProgramIdAndEmployeeId(int trainingId, int employeeId)
        {
            var list = _db.EmployeeTrainings.Where(x => x.TrainingProgramId == trainingId && x.EmployeeId == employeeId).ToList();
            _db.EmployeeTrainings.RemoveRange(list);
            _db.SaveChanges();
        }

        public void DeleteByTrainingProgramId(int trainingId)
        {
            var list = _db.EmployeeTrainings.Where(x => x.TrainingProgramId == trainingId).ToList();
            _db.EmployeeTrainings.RemoveRange(list);
            _db.SaveChanges();
        }
    }
}
