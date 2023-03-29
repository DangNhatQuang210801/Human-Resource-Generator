using Human_Resource_Generator.Data;
using Human_Resource_Generator.Interfaces;
using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Repository
{
    public class EmployeeRepo : IEmployee
    {
        private readonly ApplicationDbContext _db;

        public EmployeeRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Delete(Employee employee)
        {
            _db.Employees.Remove(employee);
        }

        public List<Employee> GetAll()
        {
            return _db.Employees.ToList();
        }

        public Employee GetById(int employee_id)
        {
            return _db.Employees.FirstOrDefault(x => x.employee_id == employee_id);
        }

        public void Insert(Employee employee)
        {
            _db.Employees.Add(employee);
        }

        public void Update(Employee employee)
        {
            _db.Employees.Update(employee);
        }
    }
}
