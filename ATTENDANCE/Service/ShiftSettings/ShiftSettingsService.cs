using ATTENDANCE.DTO.Request;
using ATTENDANCE.DTO.Response;
using ATTENDANCE.DTO.Response.shift;
using ATTENDANCE.Repository.ShiftSettings;
using EMPLOYEE_INFORMATION.Models.Entity;
using System.Runtime.InteropServices;

namespace ATTENDANCE.Service.ShiftSettings
{
    public class ShiftSettingsService(IShiftSettingsRepository repository):IShiftSettingsService
    {
        public async Task<string> CreateNormalShiftAsync(ShiftInsertRequestDto Request)
        {
           return await repository.CreateNormalShiftAsync(Request);
        }

        public async Task<string> CreateOpenShiftAsync(ShiftInsertRequestDto Request)
        {
           return await repository.CreateOpenShiftAsync(Request);
        }

        public async Task<string> CreateSplitShiftAsync(ShiftInsertRequestDto Request)
        {
            return await repository.CreateSplitShiftAsync(Request);
        }

        public async Task<List<FillAllShiftDto>> FillAllShift(ShiftViewDto Request)
        {
            return await repository.FillAllShift(Request);
        }

          

        public async Task<List<HrmValueType>> GetShiftDayTypesAsync()
        {
            return await repository.GetShiftDayTypesAsync();
        }

        public Task<int> InsertShiftNormalSeasonAsync(ShiftInsertRequestDto Request)
        {
            return repository.InsertShiftNormalSeasonAsync(Request);
        }

        public async Task<(int ErrorID, string ErrorMessage)> UpdateOpenShiftAsync(ShiftInsertRequestDto Request)
        {
           return await repository.UpdateOpenShiftAsync(Request);
        }

        //public async Task<ShiftDetailsCollectionDto> ViewAllshiftAsync(int shiftId, int entryBy, int roleId)
        //{
        //    return await repository.ViewAllshiftAsync( shiftId, entryBy, roleId);
        //}
    }
}
