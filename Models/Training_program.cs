namespace Human_Resource_Generator.Models
{
    public class Training_program
    {

        public int program_id { get; set; }
        public string program_name { get; set; }
        public string program_description { get; set; }
        public DateOnly date_of_program { get; set; }
        public List<Employee_Training> employees_Training { get; set; }

    }
}
