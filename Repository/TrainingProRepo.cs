using Human_Resource_Generator.Data;
using Human_Resource_Generator.Interfaces;
using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Repository
{
    public class TrainingProRepo : ITrainingProgram
    {
        private readonly ApplicationDbContext _db;

        public TrainingProRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Delete(TrainingProgram trainingprogram)
        {
            _db.TrainingPrograms.Remove(trainingprogram);
        }

        public List<TrainingProgram> GetAll()
        {
            return _db.TrainingPrograms.ToList();
        }

        public TrainingProgram GetById(string program_id)
        {
            return _db.TrainingPrograms.FirstOrDefault(x => x.program_id == program_id);
        }

        public void Insert(TrainingProgram trainingprogram)
        {
            _db.TrainingPrograms.Add(trainingprogram);
        }

        public void Update(TrainingProgram trainingprogram)
        {
            _db.TrainingPrograms.Update(trainingprogram);
        }
    }
}
