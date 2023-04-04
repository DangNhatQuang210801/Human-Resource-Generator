using MessagePack;

namespace Human_Resource_Generator.Models
{
    public class Training_program
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int program_id { get; set; }
        public string program_name { get; set; }
        public string program_description { get; set; }
        public DateTime date_of_program { get; set; } = DateTime.Now;
        public List<Employee_Training> employees_Training { get; set; }

    }
}
