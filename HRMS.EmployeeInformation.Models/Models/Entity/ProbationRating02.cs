namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class ProbationRating02
{
    public int ProbRateId2 { get; set; }

    public int? ProbRateId { get; set; }

    public int? RemarkerId { get; set; }

    public string? Remarks { get; set; }

    public string? RemarkStatus { get; set; }

    public DateTime? NextRemarkDate { get; set; }

    public int? RuleOrder { get; set; }
}
