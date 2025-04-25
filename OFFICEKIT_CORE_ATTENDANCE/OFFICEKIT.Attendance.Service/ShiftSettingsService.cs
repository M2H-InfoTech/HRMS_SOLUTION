using EMPLOYEE_INFORMATION.Models.Entity;
using OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Interfaces;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Service
{
    public class ShiftSettingsService(IShiftSettingsRepository shiftSettingsRepository) : IShiftSettingsService
    {
        public async Task<List<HrmValueType>> FillShiftDayType()
        {
            return await shiftSettingsRepository.FillShiftDayType();
        }
    }
}
