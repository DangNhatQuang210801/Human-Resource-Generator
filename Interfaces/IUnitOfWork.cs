namespace Human_Resource_Generator.Interfaces
{
    public interface IUnitOfWork
    {
        IEmployee Employee { get; }
        ITrainingProgram TrainingProgram { get; }
        void Save();

    }
}
