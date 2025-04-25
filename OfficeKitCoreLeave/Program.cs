using EMPLOYEE_INFORMATION.Data;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using OFFICEKITCORELEAVE.OfficeKit.Leave.Mapping;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.Repository;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(LeaveMappings));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
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
builder.Services.AddDbContextFactory<EmployeeDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
