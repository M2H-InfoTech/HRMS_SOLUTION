namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class TabAccessRight
{
    public long AccessId { get; set; }

    public int? TabId { get; set; }

    public int? ModuleId { get; set; }

    public int? RoleId { get; set; }

    public int? TabOrNot { get; set; }
}
