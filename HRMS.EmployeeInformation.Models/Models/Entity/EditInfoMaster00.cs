namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class EditInfoMaster00
{
    public int InfoId { get; set; }

    public string? InfoCode { get; set; }

    public string? Description { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
