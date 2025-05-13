using ATTENDANCE.DTO.Request;
using ATTENDANCE.DTO.Response;
using ATTENDANCE.DTO.Response.shift;
using EMPLOYEE_INFORMATION.Models.Entity;

namespace ATTENDANCE.Repository.ShiftSettings
{
    public interface IShiftSettingsRepository
    {
        Task<List<HrmValueType>> GetShiftDayTypesAsync();
        //Task<ShiftDetailsCollectionDto> ViewAllshiftAsync(int shiftId, int entryBy, int roleId);
        Task<string> CreateSplitShiftAsync(ShiftInsertRequestDto Request);
        Task<string> CreateNormalShiftAsync(ShiftInsertRequestDto Request);
        Task<string> CreateOpenShiftAsync(ShiftInsertRequestDto Request);

        Task<(int ErrorID, string ErrorMessage)> UpdateOpenShiftAsync(ShiftInsertRequestDto Request);
        Task<int> InsertShiftNormalSeasonAsync(ShiftInsertRequestDto Request);

        Task<List<FillAllShiftDto>> FillAllShift(ShiftViewDto Request);

    }
}
