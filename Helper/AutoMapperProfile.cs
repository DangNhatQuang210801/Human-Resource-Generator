using AutoMapper;
using Human_Resource_Generator.Models;
using Human_Resource_Generator.ViewModels.EmployeeViewModels;
using Human_Resource_Generator.ViewModels.TrainingProgramViewModel;

namespace Human_Resource_Generator.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateTrainingProgramViewModel, TrainingProgram>();
            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<DetailTrainingProgramViewModel, TrainingProgram>();
            CreateMap<TrainingProgram, DetailTrainingProgramViewModel>();
            CreateMap<TrainingProgram, EditTrainingProgramViewModel>();
        }
    }
}
