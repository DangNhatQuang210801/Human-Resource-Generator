using MessagePack;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.ViewModels.TrainingProgramViewModel
{
    [Keyless]
    public class CreateTrainingProgramViewModel
    { 
        public string program_name { get; set; }
        public string program_description { get; set; }
        public DateTime date_of_program { get; set; }
    }
}
