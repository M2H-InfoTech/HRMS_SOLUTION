using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrEmpQualificationApprl
{
    public int InstId { get; set; }

    public int QlfId { get; set; }

    public int EmpId { get; set; }

    public string? Course { get; set; }

    public string? University { get; set; }

    public string? InstName { get; set; }

    public DateTime? DurFrm { get; set; }

    public DateTime? DurTo { get; set; }

    public string? YearPass { get; set; }

    public string? MarkPer { get; set; }

    public string? Subjects { get; set; }

    public int EntryBy { get; set; }

    public DateTime EntryDt { get; set; }

    public string? RequestId { get; set; }

    public string? Status { get; set; }

    public string? FlowStatus { get; set; }

    public DateTime? DateFrom { get; set; }

    public string? Class { get; set; }

    public int? MasterId { get; set; }

    public int? CourseId { get; set; }

    public int? UniversityId { get; set; }

    public int? InstitId { get; set; }

    public int? SpecialId { get; set; }
}
