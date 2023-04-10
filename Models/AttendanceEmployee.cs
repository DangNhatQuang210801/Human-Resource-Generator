using System.ComponentModel.DataAnnotations;

namespace Human_Resource_Generator.Models
{
    public class AttendanceEmployee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AttendanceId { get; set; }
        public Attendance Attendance { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int? Score { get; set; }
        public DateTime AttendanceAt { get; set; }

    }
}
