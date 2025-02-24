using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class UniversityMaster
    {
    public int UniId { get; set; }

    public string? UniversityDescription { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
    }
