using Human_Resource_Generator.Data;
using Human_Resource_Generator.Interfaces;
using Human_Resource_Generator.Models;
using Microsoft.EntityFrameworkCore;

namespace Human_Resource_Generator.Repository
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly ApplicationDbContext _db;

        public EmployeeRepo(ApplicationDbContext db)
        {
            _db = db;
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
            return _db.Employees.Where(e => e.Name.Contains(name) || e.Code.Contains(name) || e.Position.Contains(name)).ToList();
        }

        public int CountAllEmployee()
        {
            return _db.Employees.Count();
        }

        public int? GetEmployeeIdByCode(string code)
        {
            var id = _db.Employees.FirstOrDefault(e => e.Code == code)?.Id;
            return id;
        }

        public List<Employee> GetEmployeesByListCodes(List<string> listCodes)
        {
            var res = _db.Employees.Where(e => listCodes.Contains(e.Code)).ToList();
            return res;
        }
    }
}
