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
    public int EmployeeTrainingId { get; set; }
    public EmployeeTraining EmployeeTraining { get; set; }
    public DateTime AttendanceDate { get; set; }
    public bool IsJoined { get; set; }
    public int? Score { get; set; }

}