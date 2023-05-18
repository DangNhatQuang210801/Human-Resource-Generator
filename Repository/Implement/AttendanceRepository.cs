using Human_Resource_Generator.Data;
using Human_Resource_Generator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;

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
            var existAttendance = _db.Attendances.FirstOrDefault(x => x.TrainingProgramId == attendance.TrainingProgramId && x.AttendanceDate.Date == attendance.AttendanceDate.Date);
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

        public Attendance? GetById(int id)
        {
            return _db.Attendances.Include(x => x.AttendanceEmployees).FirstOrDefault(x => x.Id == id);
        }

        public void Delete(Attendance attendance)
        {
            _db.Attendances.Remove(attendance);
            _db.SaveChanges();
        }

        public void Update(Attendance attendance)
        {
            _db.Attendances.Update(attendance);
            _db.SaveChanges();
        }
        
        
    }
}
