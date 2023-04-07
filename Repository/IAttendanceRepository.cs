using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Repository
{
    public interface IAttendanceRepository
    {
        public int Add(Attendance attendance);
        public List<Attendance> GetAllByTrainingProgramId(int trainingId);
    }
}
