using System.Text;
using EMPLOYEE_INFORMATION.Data;
using EMPLOYEE_INFORMATION.Helpers;
using EMPLOYEE_INFORMATION.Services.Mapping;
using HRMS.EmployeeInformation.Repository.Common;
using HRMS.EmployeeInformation.Repository.Common.DocUpload;
using HRMS.EmployeeInformation.Repository.Common.RepositoryB;
using HRMS.EmployeeInformation.Repository.Common.RepositoryC;
using HRMS.EmployeeInformation.Service.Interface;
using HRMS.EmployeeInformation.Service.InterfaceB;
using HRMS.EmployeeInformation.Service.InterfaceC;
using HRMS.EmployeeInformation.Service.Service;
using HRMS.EmployeeInformation.Service.ServiceB;
using HRMS.EmployeeInformation.Service.ServiceC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MPLOYEE_INFORMATION.DTO.DTOs;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();
        builder.Services.AddDbContextFactory<EmployeeDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IRepositoryB, RepositoryB>();
        builder.Services.AddScoped<IRepositoryC, RepositoryC>();
        builder.Services.AddScoped<IEmployeeInformationService, EmployeeInformationService>();
        builder.Services.AddScoped<IEmployeeInformationServiceB, EmployeeInformationServiceB>();
        builder.Services.AddScoped<IEmployeeInformationServiceC, EmployeeInformationServiceC>();
        builder.Services.AddSingleton<TokenService>();
        builder.Services.AddMemoryCache();
        builder.Services.Configure<EmployeeSettings>(builder.Configuration.GetSection("EmployeeSettings"));
        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmployeeSettings>>().Value);
        builder.Services.AddAutoMapper(typeof(EmployeeMapper));
        builder.Services.AddScoped<IDocUploadRepository, DocUploadRepository>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddSwaggerGen(options =>
        {
            // Add JWT bearer definition
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid JWT token.\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR..."
            });

            // Apply security globally
            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
        }); builder.Services.AddAuthentication(options =>
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
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"🔐 JWT Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                }
            };

        });
        //builder.Services.AddMemoryCache();
        builder.Services.AddAuthorization();

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
        Log.Logger = new LoggerConfiguration()
       .MinimumLevel.Warning()  // Log only Warning and Error levels
       .WriteTo.File("logs/app.log",
           rollingInterval: RollingInterval.Day,
           outputTemplate: "Method: {SourceContext} | Time: {Timestamp:MM/dd/yyyy HH:mm:ss} | Exception: {Exception} | Message: {Message}{NewLine}"
       )
       .CreateLogger();

        // Apply Serilog as the logger
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();


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
        //app.UseExceptionHandler(errorApp =>
        //{
        //    errorApp.Run(async context =>
        //    {
        //        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        //        var ex = exceptionHandlerPathFeature?.Error;

        //        if (ex is SqlException sqlEx)
        //        {
        //            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        //            logger.LogError("Method: {MethodName} | Time: {Timestamp} | Error: {ErrorMessage}",
        //                exceptionHandlerPathFeature.Path, DateTime.Now, sqlEx.Message);

        //            //logger.LogError($"Login failed. Method: {exceptionHandlerPathFeature.Path}, Date & Time: {DateTime.Now}, Error: {sqlEx.Message}");
        //        }
        //    });
        //});
        app.Run();
    }
}