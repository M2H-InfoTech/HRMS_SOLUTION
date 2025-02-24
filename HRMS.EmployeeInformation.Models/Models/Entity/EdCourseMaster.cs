using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class EdCourseMaster
    {
    public int CourseId { get; set; }

    public int? EducId { get; set; }

    public string? CourseDesc { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
    }
