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
            return _db.Employees.Include("EmployeeTrainings.TrainingProgram").FirstOrDefault(x => x.Id == id) ?? new Employee();
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
    }
}
