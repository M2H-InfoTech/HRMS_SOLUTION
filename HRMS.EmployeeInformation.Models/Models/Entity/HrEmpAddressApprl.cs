using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrEmpAddressApprl
{
    public int InstId { get; set; }

    public int AddId { get; set; }

    public int EmpId { get; set; }

    public string? Add1 { get; set; }

    public string? Add2 { get; set; }

    public string? Add3 { get; set; }

    public int? Country { get; set; }

    public string? Pbno { get; set; }

    public string? Phone { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public string? PersonalEmail { get; set; }

    public string? AddType { get; set; }

    public int EntryBy { get; set; }

    public DateTime EntryDt { get; set; }

    public string? Extension { get; set; }

    public string? OfficePhone { get; set; }

    public string? Status { get; set; }

    public string? FlowStatus { get; set; }

    public string? RequestId { get; set; }

    public DateTime? DateFrom { get; set; }

    public int? MasterId { get; set; }
}
