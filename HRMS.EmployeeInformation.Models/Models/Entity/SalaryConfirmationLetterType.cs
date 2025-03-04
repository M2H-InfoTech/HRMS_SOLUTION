namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class SalaryConfirmationLetterType
{
    public int SalLetterId { get; set; }

    public int LetterReqId { get; set; }

    public string BankName { get; set; } = null!;

    public string BranchName { get; set; } = null!;

    public string? AccountNo { get; set; }

    public string? IdentificationCode { get; set; }

    public string? Location { get; set; }
}
