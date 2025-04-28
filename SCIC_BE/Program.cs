using Microsoft.EntityFrameworkCore;
using SCIC_BE.Controllers.StudentControllers;
using SCIC_BE.Data;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.UserRepository;
using SCIC_BE.Repository.StudentRepository;
using SCIC_BE.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddMvc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ScicDbContext>(options => options.UseSqlServer(  builder.
                                                                                Configuration.
                                                                                GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });


//Dependency Injection declare
builder.Services.AddScoped<IStudentInfoRepository,StudentInfoRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<UserRepository>();                // Đăng ký UserRepository
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<UserInfoService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ScicDbContext>();

    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() =>
{
    app.Services.GetRequiredService<ILogger<Program>>()
        .LogInformation("Backend is ready! Access Swagger UI at: https://localhost:8081/swagger/index.html");
});

app.Run();
