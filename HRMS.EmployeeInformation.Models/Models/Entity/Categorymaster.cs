namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class Categorymaster
{
    public int EntityId { get; set; }

    public int? InstId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public int? EntryBy { get; set; }

    public string? Description { get; set; }

    public int? SortOrder { get; set; }

    public bool? Mandatory { get; set; }

    public bool? CodeRequired { get; set; }

    public bool? RestrictDuplicate { get; set; }

    public bool? Grievance { get; set; }

    public int? CatTrxTypeId { get; set; }

    public int? IsCompany { get; set; }
}
