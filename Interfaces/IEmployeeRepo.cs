using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Interfaces
{
    public interface IEmployeeRepo
    {
        public List<Employee> GetAll();
        public Employee GetById(int Id);
        public void Insert(Employee employee);
        public void Update(Employee employee);
        public void Delete(int id);
    }
}
