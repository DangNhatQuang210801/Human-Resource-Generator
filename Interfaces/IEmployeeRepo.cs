using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Interfaces
{
    public interface IEmployeeRepo
    {
        public IEnumerable<Employee> GetAll();
        public Employee GetById(int id);
        public void Insert(Employee employee);
        public void Update(Employee employee);
        public void Delete(int id);
        public List<Employee> GetListDataByListId(List<int> listId);
        public List<Employee> GetEmployeesByName(string name);
    }
}
