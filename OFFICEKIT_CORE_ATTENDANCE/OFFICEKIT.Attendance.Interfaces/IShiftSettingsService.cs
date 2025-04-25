using EMPLOYEE_INFORMATION.Models.Entity;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Interfaces
{
    public interface IShiftSettingsService
    {
        Task<List<HrmValueType>> FillShiftDayType();
    }
}
