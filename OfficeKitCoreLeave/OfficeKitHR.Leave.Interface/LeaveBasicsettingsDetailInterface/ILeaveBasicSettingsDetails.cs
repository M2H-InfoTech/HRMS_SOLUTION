using OFFICEKITCORELEAVE.OfficeKitHR.Leave.DTO;
using OFFICEKITCORELEAVE.OfficeKitHR.Leave.MODELS;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.Interface.HrmLeaveBasicsettingsDetailInterface
{
    public interface ILeaveBasicSettingsDetails
    {
        Task<int> SaveLeaveBasicSettingsDetails (HrmLeaveBasicsettingsDetailDto LeaveBasicSettingsDetails);
    }
}
