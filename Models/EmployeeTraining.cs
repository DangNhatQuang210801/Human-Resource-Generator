﻿using System.ComponentModel.DataAnnotations;
namespace Human_Resource_Generator.Models

{
    public class EmployeeTraining
    {
        public string ID { get; set; }
        public Employee Employee { get; set; }
        public string program_id { get; set; }
        public TrainingProgram TrainingProgram { get; set; }
    }
}


