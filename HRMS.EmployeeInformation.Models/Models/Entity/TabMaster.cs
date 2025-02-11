namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class TabMaster
{
    public long TabId { get; set; }

    public string? TabName { get; set; }

    public int? ModuleId { get; set; }

    public string? Code { get; set; }

    public int? TabOrNot { get; set; }

    public string? Active { get; set; }
}
