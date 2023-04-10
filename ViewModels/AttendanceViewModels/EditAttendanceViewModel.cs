using Human_Resource_Generator.Models;

namespace Human_Resource_Generator.ViewModels.AttendanceViewModels
{
    public class EditAttendanceViewModel : TrainingProgram
    {
        public Attendance Attendance { get; set; }
        public List<Employee> JoinedEmployees { get; set; }
        public List<AttendanceEmployee> AttendanceEmployees { get; set; }
    }
}
