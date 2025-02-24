using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class EducationMaster
    {
    public int EducId { get; set; }

    public string? EduDescription { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
    }
