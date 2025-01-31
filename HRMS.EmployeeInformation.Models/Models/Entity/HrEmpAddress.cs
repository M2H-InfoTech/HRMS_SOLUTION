using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class HrEmpAddress
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

    public string? OfficialEmail { get; set; }

    public string? PersonalEmail { get; set; }

    public string? AddType { get; set; }

    public int EntryBy { get; set; }

    public DateTime EntryDt { get; set; }

    public string? Extension { get; set; }

    public string? OfficePhone { get; set; }

    public int? ApprlId { get; set; }

    public string? HomeCountryPhone { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
