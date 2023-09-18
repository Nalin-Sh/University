using Application;
using Infrastructures.Repository;
using Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using Infrastructures.Data;
using Domain.Models;
using Infrastructures.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication()
    .AddPresentation()
    .AddInfrastructures();
builder.Host.UseSerilog((context, configuration) =>
 configuration.ReadFrom.Configuration(context.Configuration)

);

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<FacultyServices>();
builder.Services.AddScoped<CourseServices>();
builder.Services.AddScoped<StudentServices>();
builder.Services.AddScoped<CourseAssignmentServices>();
builder.Services.AddScoped<EnrollmentServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
