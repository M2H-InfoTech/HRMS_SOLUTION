using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class EmpQualification
{
    public int QualId { get; set; }

    public string? EmployeeCode { get; set; }

    public string? Course { get; set; }

    public string? Board { get; set; }

    public string? Institution { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? Percentage { get; set; }

    public string? Class { get; set; }

    public string? Subjects { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
