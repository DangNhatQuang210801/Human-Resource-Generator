using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Interfaces
{
    public interface ITrainingProgram 
    {
        List<TrainingProgram> GetAll();
        TrainingProgram GetById(int program_id);
        void Insert(TrainingProgram trainingprogram);
        void Update(TrainingProgram trainingprogram);
        void Delete(TrainingProgram trainingprogram);
    }
}
