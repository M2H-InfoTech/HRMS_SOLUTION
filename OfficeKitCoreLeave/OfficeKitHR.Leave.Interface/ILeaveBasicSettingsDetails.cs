using OFFICEKITCORELEAVE.OfficeKitHR.Leave.MODELS;

namespace OFFICEKITCORELEAVE.OfficeKitHR.Leave.Interface
{
    public interface ILeaveBasicSettingsDetails
    {
        Task<int> SaveLeaveBasicSettingsDetails(HrmLeaveMasterandsettingsLink LeaveBasicSettingsDetails);
    }
}
