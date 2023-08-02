using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Human_Resource_Generator.Data;
using Microsoft.AspNetCore.Mvc;

namespace Human_Resource_Generator.Models;

[Table("TrainingPrograms")]
public class TrainingProgram
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set;}
    public string Description { get; set; }
    [Required]
    public string Teacher { get; set; }
    [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<EmployeeTraining> EmployeeTrainings { get; set; }
    public ICollection<Attendance> Attendances { get; set; }
}