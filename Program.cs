using AutoMapper;
using Human_Resource_Generator.Data;
using Human_Resource_Generator.Helper;
using Human_Resource_Generator.Interfaces;
using Human_Resource_Generator.Repository;
using Human_Resource_Generator.Repository.Implement;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
builder.Services.AddScoped<ITrainingProgramRepository,TrainingProgramRepository>();
builder.Services.AddScoped<IEmployeeTrainingRepository, EmployeeTrainingRepository>();

builder.Services.AddControllersWithViews();

// configure automapper with all automapper profiles from this assembly
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
