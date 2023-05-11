using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.Repository
{
    public interface IAttendanceEmployeeRepository
    {
        public int Add(AttendanceEmployee attendanceEmployee);
        public List<AttendanceEmployee> GetByAttendanceId(int attendanceId);

        public void DeleteByAttendanceIdAndEmployeeId(int attendanceId, int employeeId);

        public int CountAttendanceEmployeesByEmployeeIdAndContainingListAttendanceId(int employeeId,
            List<int> listAttendanceIds);
    }
}
