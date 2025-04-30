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
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
