namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class AdmUserMaster
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string DetailedName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? ContactNo { get; set; }

    public DateTime EntryDate { get; set; }

    public string Active { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Email { get; set; }

    public int? UploadEmpId { get; set; }

    public int? VerificationType { get; set; }

    public bool? NeedApp { get; set; }

    public int? CountryId { get; set; }

    public int? CurrencyId { get; set; }

    public DateTime? FirstLoginTime { get; set; }

    public string? UserType { get; set; }

    public int? LangId { get; set; }
}
