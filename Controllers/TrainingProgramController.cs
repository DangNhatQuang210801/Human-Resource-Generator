using Human_Resource_Generator.Interfaces;
using Human_Resource_Generator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Human_Resource_Generator.Controllers;

public class TrainingProgramController : Controller
{
    private readonly ITrainingProgram _trainingProgramRepo;

    public TrainingProgramController(ITrainingProgram trainingProgramRepo)
    {
        _trainingProgramRepo = trainingProgramRepo;
    }

    public TrainingProgram GetTrainingProgramsById(string id)
    {
        return _trainingProgramRepo.GetById(id);
    }
}