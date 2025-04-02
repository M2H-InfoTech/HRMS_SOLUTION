namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class Subcategory
{
    public int SubEntityId { get; set; }

    public int? InstId { get; set; }

    public string? Code { get; set; }

    public int EntityId { get; set; }

    public string? Description { get; set; }

    public bool? LinkToEmp { get; set; }

    public DateTime? TransactionDate { get; set; }

    public int? EntryBy { get; set; }

    public bool? Grievance { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? IsDuplicate { get; set; }

    public string? DisplayEntName { get; set; }
}
