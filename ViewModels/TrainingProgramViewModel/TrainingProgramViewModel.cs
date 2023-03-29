using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Human_Resource_Generator.ViewModels.TrainingProgramViewModels
{
    public class TrainingProgramViewModel
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int program_id { get; set; }
        public string program_name { get; set; }
        public string program_description { get; set; }
        public DateTime date_of_program { get; set; }
    }
}
