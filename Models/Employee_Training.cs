using MessagePack;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.Models;

//public class Employee
//{
//    public int employee_id { get; set; }
//    public string employee_name { get; set; }
//    public string employee_department { get; set; }
//    public DateOnly date_of_birth { get; set; }
//    public List<Training_program> Training_programs { get; set; }
//}
//public class Training_program
//{
//    public int program_id { get; set; }
//    public string program_name { get; set;}
//    public string program_description { get; set; }
//    public DateOnly date_of_program { get; set; }
//    public List<Employee> employees { get; set; }
//}
public class Employee_Training
{

    public int employee_id { get; set; }
    public Employee employee { get; set; }
    public int program_id { get; set; }
    public Training_program training_Program { get; set; }
    public string program_manager { get; set; }
    
}
