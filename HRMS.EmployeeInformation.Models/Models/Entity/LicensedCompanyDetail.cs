namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class LicensedCompanyDetail
{
    public int LicenseId { get; set; }

    public string? DomainName { get; set; }

    public int? EmployeeLimit { get; set; }

    public int? EntityLimit { get; set; }

    public string? CompanyName { get; set; }

    public int? LicenseCode { get; set; }
}
