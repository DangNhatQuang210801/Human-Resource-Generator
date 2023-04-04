using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Interfaces
{
    public interface ITrainingProgram 
    {
        public List<TrainingProgram> GetAll();
        public TrainingProgram? GetById(int programId);
        public void Add(TrainingProgram trainingProgram);
        public void Update(TrainingProgram trainingProgram);
        public void Delete(TrainingProgram trainingProgram);
    }
}
