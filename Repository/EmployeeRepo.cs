using Human_Resource_Generator.Data;
using Human_Resource_Generator.Interfaces;
using Human_Resource_Generator.Models;
using Microsoft.EntityFrameworkCore;

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

        public Employee GetById(string ID)
        {
            return _db.Employees.Include("EmployeeTrainings.TrainingProgram").FirstOrDefault(x => x.ID == ID);
        }

        public void Insert(Employee employee)
        {
            employee.ID = Guid.NewGuid().ToString();
            _db.Employees.Add(employee);
        }

        public void Update(Employee employee)
        {
            _db.Employees.Update(employee);
        }
    }
}
