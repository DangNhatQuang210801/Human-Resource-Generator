using Human_Resource_Generator.Data;
using Human_Resource_Generator.Models;
using Microsoft.EntityFrameworkCore;

namespace Human_Resource_Generator.Repository.Implement
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _db;

        public AttendanceRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public int Add(Attendance attendance)
        {
            var existAttendance = _db.Attendances.FirstOrDefault(x => x.TrainingProgramId == attendance.TrainingProgramId && x.AttendanceDate == attendance.AttendanceDate);
            if (existAttendance != null)
            {
                return -1;
            }
            _db.Attendances.Add(attendance);
            _db.SaveChanges();
            return attendance.Id;
        }
        public List<Attendance> GetAllByTrainingProgramId(int trainingId)
        {
            return _db.Attendances.Include(x => x.AttendanceEmployees).Where(x => x.TrainingProgramId.Equals(trainingId)).ToList();
        }

    }
}
