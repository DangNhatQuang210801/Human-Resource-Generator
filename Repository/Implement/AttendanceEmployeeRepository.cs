using Human_Resource_Generator.Data;
using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Repository.Implement
{
    public class AttendanceEmployeeRepository : IAttendanceEmployeeRepository
    {
        private readonly ApplicationDbContext _db;

        public AttendanceEmployeeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public int Add(AttendanceEmployee attendanceEmployee)
        {
            var existAttendanceEmployee = _db.AttendanceEmployees.FirstOrDefault(x => x.AttendanceId == attendanceEmployee.AttendanceId && x.EmployeeId == attendanceEmployee.EmployeeId);
            if (existAttendanceEmployee != null)
            {
                existAttendanceEmployee.Score = attendanceEmployee.Score;
                _db.AttendanceEmployees.Update(existAttendanceEmployee);
                _db.SaveChanges();
                return -1;
            }
            _db.AttendanceEmployees.Add(attendanceEmployee);
            _db.SaveChanges();
            return attendanceEmployee.Id;
        }

        public void DeleteByAttendanceIdAndEmployeeId(int attendanceId, int employeeId)
        {
            var existed = _db.AttendanceEmployees.FirstOrDefault(x => x.AttendanceId == attendanceId && x.EmployeeId == employeeId);
            if (existed != null)
            {
                _db.AttendanceEmployees.Remove(existed);
                _db.SaveChanges();
            }
        }

        public List<AttendanceEmployee> GetByAttendanceId(int attendanceId)
        {
            return _db.AttendanceEmployees.Where(x => x.AttendanceId == attendanceId).ToList();
        }
    }
}
