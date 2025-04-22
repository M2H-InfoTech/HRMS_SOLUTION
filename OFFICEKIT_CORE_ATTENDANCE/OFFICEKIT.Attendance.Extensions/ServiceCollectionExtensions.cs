using Microsoft.Extensions.DependencyInjection;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Interfaces;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Repository;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            services.AddScoped<IAttendanceLogRepository, AttendanceLogRepository>();
            return services;
        }
    }
}
