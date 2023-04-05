using Human_Resource_Generator.Models;
using MessagePack;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.ViewModels.TrainingProgramViewModel
{
    [Keyless]
    public class CreateTrainingProgramViewModel : TrainingProgram
    { 
        public List<Employee> Employees { get; set; }
    }
}
