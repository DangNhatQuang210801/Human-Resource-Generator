using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.Models;

public class Attendance
{
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int TrainingProgramId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public TrainingProgram TrainingProgram { get; set; }
    public ICollection<AttendanceEmployee> AttendanceEmployees { get; set; }

}