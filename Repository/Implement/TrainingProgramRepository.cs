﻿using Human_Resource_Generator.Data;
using Human_Resource_Generator.Models;
using Human_Resource_Generator.ViewModels.TrainingProgramViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using NuGet.DependencyResolver;
using System.Xml.Linq;

namespace Human_Resource_Generator.Repository.Implement
{
    public class TrainingProgramRepository : ITrainingProgramRepository
    {
        private readonly ApplicationDbContext _db;

        public TrainingProgramRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Delete(TrainingProgram trainingProgram)
        {
            _db.TrainingPrograms.Remove(trainingProgram);
            _db.SaveChanges();
        }

        public List<TrainingProgram> GetAll()
        {
            return _db.TrainingPrograms.Include(x => x.EmployeeTrainings).ToList();
        }

        public TrainingProgram? GetById(int programId)
        {
            return _db.TrainingPrograms.FirstOrDefault(x => x.Id == programId);
        }

        public int Add(TrainingProgram trainingProgram)
        {
            var existTraining = _db.TrainingPrograms.FirstOrDefault(x => x.Name == trainingProgram.Name);
            if (existTraining != null)
            {
                return -1;
            }
            _db.TrainingPrograms.Add(trainingProgram);
            _db.SaveChanges();
            return trainingProgram.Id;
        }

        public void Update(TrainingProgram trainingProgram)
        {
            _db.TrainingPrograms.Update(trainingProgram);
            _db.SaveChanges();
        }

        public List<TrainingProgram> GetAllByFilter(string? name)
        {
            var trainingPrograms = _db.TrainingPrograms.Include(x => x.EmployeeTrainings).ToList();
            if (!string.IsNullOrWhiteSpace(name))
            {
                trainingPrograms = trainingPrograms.Where(t => t.Name.Contains(name)
                                                              || t.Description.Contains(name)
                                                              || name.Contains(t.Name)
                                                              || name.Contains(t.Description)
                                                              || t.Teacher.Contains(name)
                                                              || name.Contains(t.Teacher)
                                                              || t.CreatedAt.Year.ToString().Contains(name)
                                                              || name.Contains(t.CreatedAt.Year.ToString())).ToList();
            }

            return trainingPrograms;
        }

    }
}
