using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Repository
{
    public interface ITrainingProgramRepository
    {
        public List<TrainingProgram> GetAll();
        public TrainingProgram? GetById(int programId);
        public void Add(TrainingProgram trainingProgram);
        public void Update(TrainingProgram trainingProgram);
        public void Delete(TrainingProgram trainingProgram);
    }
}
