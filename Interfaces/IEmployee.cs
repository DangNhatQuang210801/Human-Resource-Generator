using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Interfaces
{
    public interface IEmployee
    {
        List<Employee> GetAll();
        Employee GetById(string employee_id);
        void Insert(Employee employee);
        void Update(Employee employee);
        void Delete(Employee employee);
    }
}
