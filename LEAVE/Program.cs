using System.Text;
using EMPLOYEE_INFORMATION.Data;
using LEAVE.Helpers;
using LEAVE.Helpers.AccessMetadataService;
using LEAVE.Repository.AssignLeave;
using LEAVE.Repository.BasicSettings;
using LEAVE.Repository.LeaveBalance;
using LEAVE.Repository.LeaveMaster;
using LEAVE.Repository.LeavePolicy;
using LEAVE.Service.AssignLeave;
using LEAVE.Service.BasicSettings;
using LEAVE.Service.LeaveBalance;
using LEAVE.Service.LeaveMaster;
using LEAVE.Service.LeavePolicy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
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
});
builder.Services.AddHttpContextAccessor();
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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
builder.Services.AddDbContextFactory<EmployeeDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<ILeaveMasterRepository, LeaveMasterRepository>();
builder.Services.AddScoped<ILeaveMasterService, LeaveMasterService>();

builder.Services.AddScoped<IAssignLeaveRepository, AssignLeaveRepository>();
builder.Services.AddScoped<IAssignLeaveService, AssignLeaveService>();

builder.Services.AddScoped<IBasicSettingsRepository, BasicSettingsRepository>();
builder.Services.AddScoped<IBasicSettingsService, BasicSettingsService>();

builder.Services.AddScoped<ILeaveBalanceRepository, LeaveBalanceRepository>();
builder.Services.AddScoped<ILeaveBalanceService, LeaveBalanceService>();

builder.Services.AddScoped<ILeavePolicyService, LeavePolicyService>();
builder.Services.AddScoped<ILeavePolicyRepository, LeavePolicyRepository>();
builder.Services.AddHttpClient<ExternalApiService>();
builder.Services.AddScoped<IAccessMetadataService, AccessMetadataService>();
builder.Services.Configure<HttpClientSettings>(builder.Configuration.GetSection("HttpClientSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<HttpClientSettings>>().Value);
var app = builder.Build();
//app.UseCors("AllowAllOrigins");
var supportedCulture = new[] { "en", "ar" };

var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture("en").AddSupportedCultures(supportedCulture).AddSupportedUICultures(supportedCulture);
app.UseRequestLocalization(localizationOptions);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
//app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();
