using EMPLOYEE_INFORMATION.Models.Entity;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Response;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Interfaces
{
    public interface IShiftSettingsRepository
    {
        Task<PaginatedResultDto> GetShiftAccessDetails(int shiftAccessId, int entryBy, int roleId, int status, int empStatus, int pageNumber, int pageSize);
        Task<List<HrmValueType>> FillShiftDayType();
    }
}
