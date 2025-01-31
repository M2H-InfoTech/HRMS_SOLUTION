using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrEmpTechnicalApprl
{
    public int InstId { get; set; }

    public int TechId { get; set; }

    public int EmpId { get; set; }

    public string? Course { get; set; }

    public string? CourseDtls { get; set; }

    public string? Year { get; set; }

    public DateTime? DurFrm { get; set; }

    public DateTime? DurTo { get; set; }

    public string? MarkPer { get; set; }

    public int EntryBy { get; set; }

    public DateTime EntryDt { get; set; }

    public string? InstName { get; set; }

    public string? RequestId { get; set; }

    public string? Status { get; set; }

    public string? FlowStatus { get; set; }

    public DateTime? DateFrom { get; set; }

    public int? MasterId { get; set; }

    public string? LangSkills { get; set; }
}
