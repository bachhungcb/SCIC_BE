using System;
using Microsoft.EntityFrameworkCore;
using SCIC_BE.Controllers.StudentControllers;
using SCIC_BE.Data;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Models;
using SCIC_BE.Repositories.UserRepository;
using SCIC_BE.Repository.StudentRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using SCIC_BE.Repositories.RoleRepository;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using SCIC_BE.Middlewares.Exceptions;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SCIC_BE.Hubs;
using SCIC_BE.Repositories.AttendanceLogRepository;
using SCIC_BE.Repositories.LecturerRepository;
using SCIC_BE.Repositories.PermissionRepository;
using SCIC_BE.Repositories.AttendanceRepository;
using SCIC_BE.Services.Server;
using SCIC_BE.Services.Thingsboard;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            
            RoleClaimType = ClaimTypes.Role, 
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new ApiError
                {
                    Status = 401,
                    Message = "Unauthorized",
                    ErrorCode = "unauthorized",
                    TraceId = context.HttpContext.TraceIdentifier,
                    Timestamp = DateTime.UtcNow.ToString("o"),
                });

                return context.Response.WriteAsync(result);
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new ApiError
                {
                    Status = 403,
                    Message = "Forbidden",
                    ErrorCode = "forbidden",
                    TraceId = context.HttpContext.TraceIdentifier,
                    Timestamp = DateTime.UtcNow.ToString("o"),
                });

                return context.Response.WriteAsync(result);
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddMvc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SCIC_BE", Version = "v1" });

    var securitySchema = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter Bearer Token: Bearer {your JWT token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});

// builder.Services.AddCors(options =>
// {
//     options.AddDefaultPolicy(policy =>
//     {
//         policy
//             .WithOrigins("*")
//             .AllowAnyHeader()
//             .AllowAnyMethod()
//             .SetIsOriginAllowed(_ => true)
//             .AllowCredentials();
//     });
// });


builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder
        //.WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials();;

}));

builder.Services.AddDbContext<ScicDbContext>(options => 
                                                    options.UseSqlServer(
                                                            builder.Configuration.GetConnectionString("DefaultConnection"),
                                                            sqlOption =>
                                                            {
                                                                sqlOption.CommandTimeout(60);
                                                            }));

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
})
.AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});


//Dependency Injection declare
builder.Services.AddHttpClient(); // Register IHttpClientFactory

builder.Services.AddScoped<IStudentInfoRepository, StudentInfoRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddScoped<ILecturerService, LecturerService>();
builder.Services.AddScoped<ILecturerRepository, LecturerRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserInfoService>();

builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();

builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();

builder.Services.AddScoped<IAttendanceLogRepository, AttendanceLogRepository>();
builder.Services.AddScoped<IAttendanceLogService, AttendanceLogService>();

builder.Services.AddScoped<RcpService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<PasswordService>();

builder.Services.AddSingleton<ThingsBoardAuthService>();
builder.Services.AddHostedService<ThingsBoardTelemetryService>();


builder.Services.AddSignalR();

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
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapHub<TelemetryHub>("/api/ws/telemetryHub");

app.Lifetime.ApplicationStarted.Register(() =>
{
    app.Services.GetRequiredService<ILogger<Program>>()
        .LogInformation("Backend is ready! Access Swagger UI at: http://localhost:8090/swagger/index.html");
});

app.Run();

