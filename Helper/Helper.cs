﻿using AutoMapper;
using Human_Resource_Generator.Models;
using Human_Resource_Generator.ViewModels.EmployeeViewModels;
using Human_Resource_Generator.ViewModels.TrainingProgramViewModel;
using Human_Resource_Generator.ViewModels.TrainingProgramViewModels;

namespace Human_Resource_Generator.Helper
{
    public class Helper : Profile
    {
        public Helper()
        {
            CreateMap<TrainingProgram, TrainingProgramViewModel>().ReverseMap();
            CreateMap<CreateTrainingProgramViewModel, TrainingProgram>();
            CreateMap<Employee, EmployeeViewModel>();
        }
    }
}