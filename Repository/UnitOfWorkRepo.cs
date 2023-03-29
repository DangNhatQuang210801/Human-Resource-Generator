using Human_Resource_Generator.Data;
using Human_Resource_Generator.Interfaces;

namespace Human_Resource_Generator.Repository
{
    public class UnitOfWorkRepo : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private IEmployee _employeeRepo;
        private ITrainingProgram _trainingProRepo;
        public UnitOfWorkRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEmployee Employee
        {
            get
            {
                return _employeeRepo = _employeeRepo ?? new EmployeeRepo(_db);
            }
        }
        public ITrainingProgram TrainingProgram
        {
            get
            {
                return _trainingProRepo = _trainingProRepo ?? new TrainingProRepo(_db);
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
