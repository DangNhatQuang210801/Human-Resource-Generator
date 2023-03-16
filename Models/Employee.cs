using MessagePack;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.Models;

public class Employee
{
    [System.ComponentModel.DataAnnotations.Key]
    public int employee_id { get; set; }
    [System.ComponentModel.DataAnnotations.Required]
    public string employee_name { get; set; }
    public string employee_department { get; set; }
    public DateTime date_of_birth { get; set; } = DateTime.Now;
    public List<Employee_Training> employees_Training { get; set; }
}
 
