using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendace.Data;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Interfaces;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Repository;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Service;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Extensions;
using EMPLOYEE_INFORMATION.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddControllers();


builder.Services.AddDbContextFactory<EmployeeDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Register DbContext
builder.Services.AddDbContext<AttendanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Register repositories directly
builder.Services.AddScoped<IAttendanceLogRepository, AttendanceLogRepository>();
builder.Services.AddScoped<IAttendanceLogService, AttendanceLogService>();
builder.Services.AddScoped<IShiftSettingsRepository, ShiftSettingsRepository>();
builder.Services.AddScoped<IShiftSettingsService, ShiftSettingsService>();


// Add OpenAPI support
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
