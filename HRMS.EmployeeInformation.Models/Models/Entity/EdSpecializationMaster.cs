using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class EdSpecializationMaster
    {
    public int EdSpecId { get; set; }

    public int? EdCourId { get; set; }

    public string? SpecializationDesc { get; set; }
    }
