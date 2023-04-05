using Human_Resource_Generator.Data;
using Human_Resource_Generator.Models;

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

        public List<EmployeeTraining> GetAll()
        {
            return _db.EmployeeTrainings.ToList();
        }

        public EmployeeTraining? GetById(int id)
        {
            return _db.EmployeeTrainings.FirstOrDefault(x => x.Id == id);
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
            return _db.EmployeeTrainings.Where(x => x.TrainingProgramId == id).ToList();
        }
    }
}
