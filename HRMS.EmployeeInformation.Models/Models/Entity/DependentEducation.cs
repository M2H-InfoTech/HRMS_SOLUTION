using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class DependentEducation
{
    public int DepEduId { get; set; }

    public int? EduId { get; set; }

    public int? CourseId { get; set; }

    public int? SpecialId { get; set; }

    public int? EmpId { get; set; }

    public int? DepId { get; set; }

    public string? UniversityEdu { get; set; }

    public string? CourseType { get; set; }

    public string? Year { get; set; }

    public int? EntryBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UniversityEduId { get; set; }
}
