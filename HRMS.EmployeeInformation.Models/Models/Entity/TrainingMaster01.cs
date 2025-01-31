using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class TrainingMaster01
{
    public int FileUpdId { get; set; }

    public int TrMasterId { get; set; }

    public string? FileUrl { get; set; }

    public string? FileName { get; set; }

    public string? FileType { get; set; }
}
