using System.Text;
using EMPLOYEE_INFORMATION.Data;
using EMPLOYEE_INFORMATION.Helpers;
using EMPLOYEE_INFORMATION.Services.Mapping;
using HRMS.EmployeeInformation.Repository.Common;
using HRMS.EmployeeInformation.Repository.Common.RepositoryA;
using HRMS.EmployeeInformation.Repository.Common.RepositoryB;
using HRMS.EmployeeInformation.Repository.Common.RepositoryC;
using HRMS.EmployeeInformation.Service.Interface;
using HRMS.EmployeeInformation.Service.InterfaceA;
using HRMS.EmployeeInformation.Service.InterfaceB;
using HRMS.EmployeeInformation.Service.InterfaceC;
using HRMS.EmployeeInformation.Service.Service;
using HRMS.EmployeeInformation.Service.ServiceA;
using HRMS.EmployeeInformation.Service.ServiceB;
using HRMS.EmployeeInformation.Service.ServiceC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MPLOYEE_INFORMATION.DTO.DTOs;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContextFactory<EmployeeDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IRepositoryA, RepositoryA>();
        builder.Services.AddScoped<IRepositoryB, RepositoryB>();
        builder.Services.AddScoped<IRepositoryC, RepositoryC>();
        builder.Services.AddScoped<IEmployeeInformationService, EmployeeInformationService>();
        builder.Services.AddScoped<IEmployeeInformationServiceA, EmployeeInformationServiceA>();
        builder.Services.AddScoped<IEmployeeInformationServiceB, EmployeeInformationServiceB>();
        builder.Services.AddScoped<IEmployeeInformationServiceC, EmployeeInformationServiceC>();
        builder.Services.AddSingleton<TokenService>();
        builder.Services.AddMemoryCache();
        builder.Services.Configure<EmployeeSettings>(builder.Configuration.GetSection("EmployeeSettings"));
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmployeeSettings>>().Value);
        builder.Services.AddAutoMapper(typeof(EmployeeMapper));
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var jwtSettings = builder.Configuration.GetSection("jwtSettings");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
            };

        });
        //builder.Services.AddMemoryCache();
        builder.Services.AddAuthorization(options => options.AddPolicy("AdminPolicy", p => p.RequireRole("Admin")));

        builder.Services.AddRateLimiter(rateLimiteroptions =>
        {
            rateLimiteroptions.AddFixedWindowLimiter("fixed", options =>
            {
                options.QueueLimit = 0;// 
                options.PermitLimit = 1;//
                options.Window = TimeSpan.FromSeconds(10);//

            });
            rateLimiteroptions.AddConcurrencyLimiter("concurrency", conOption =>
            {
                conOption.QueueLimit = 0;
                conOption.PermitLimit = 5;

            });
            rateLimiteroptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });
        builder.Services.AddLocalization(optinos => optinos.ResourcesPath = "Resources");

        //builder.WebHost.UseUrls("http://localhost:80", "https://localhost:443");





        var app = builder.Build();
        app.UseCors("AllowAllOrigins");
        var supportedCulture = new[] { "en", "ar" };

        var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture("en").AddSupportedCultures(supportedCulture).AddSupportedUICultures(supportedCulture);
        app.UseRequestLocalization(localizationOptions);
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHsts();
        }

        app.UseRateLimiter();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}