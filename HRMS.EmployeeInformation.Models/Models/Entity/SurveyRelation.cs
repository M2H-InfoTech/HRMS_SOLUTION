namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class SurveyRelation
{
    public int ProbId { get; set; }

    public int? EmpId { get; set; }

    public int? ProbationReview { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? ReviewDate { get; set; }

    public int? ReviewCount { get; set; }

    public bool? IsDelete { get; set; }
}
