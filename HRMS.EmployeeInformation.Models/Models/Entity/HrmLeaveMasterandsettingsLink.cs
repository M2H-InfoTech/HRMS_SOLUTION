namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class HrmLeaveMasterandsettingsLink
{
    public int IdMasterandSettingsLink { get; set; }

    public int? LeaveMasterId { get; set; }

    public int? SettingsId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }
}
