using ATTENDANCE.Repository.AssignPolicy;
using ATTENDANCE.Repository.AttendanceLog;
using ATTENDANCE.Repository.AttendancePolicy;
using ATTENDANCE.Repository.AttendanceProcess;
using ATTENDANCE.Repository.ShiftMasterUpload;
using ATTENDANCE.Repository.ShiftSettings;
using ATTENDANCE.Repository.ShiftUpload;
using ATTENDANCE.Service.AssignPolicy;
using ATTENDANCE.Service.AssignShift;
using ATTENDANCE.Service.AttendanceLog;
using ATTENDANCE.Service.AttendancePolicy;
using ATTENDANCE.Service.AttendanceProcess;
using ATTENDANCE.Service.ShiftMasterUpload;
using ATTENDANCE.Service.ShiftSettings;
using ATTENDANCE.Service.ShiftUpload;
using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.Repository.Common;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<EmployeeDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


//builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAssignPolicyRepository, AssignPolicyRepository>();
builder.Services.AddScoped<IAssignShiftRepository, AssignShiftRepository>();
builder.Services.AddScoped<IAttendanceLogRepository, AttendanceLogRepository>();
builder.Services.AddScoped<IAttendancePolicyRepository, AttendancePolicyRepository>();
builder.Services.AddScoped<IAttendanceProcessRepository, AttendanceProcessRepository>();
builder.Services.AddScoped<IShiftMasterUploadRepository, ShiftMasterUploadRepository>();
builder.Services.AddScoped<IShiftSettingsRepository, ShiftSettingsRepository>();
builder.Services.AddScoped<IShiftUploadRepository, ShiftUploadRepository>();



// Register Services with AddScoped
builder.Services.AddScoped<IAssignPolicyService, AssignPolicyService>();
builder.Services.AddScoped<IAssignShiftService, AssignShiftService>();
builder.Services.AddScoped<IAttendanceLogService, AttendanceLogService>();
builder.Services.AddScoped<IAttendancePolicyService, AttendancePolicyService>();
builder.Services.AddScoped<IAttendanceProcessService, AttendanceProcessService>();
builder.Services.AddScoped<IShiftMasterUploadService, ShiftMasterUploadService>();
builder.Services.AddScoped<IShiftSettingsService, ShiftSettingsService>();
builder.Services.AddScoped<IShiftUploadService, ShiftUploadService>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi




// Configure the HTTP request pipeline.
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();

}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
