using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrmsEmpdocuments02
{
    public int DocId { get; set; }

    public int? DetailId { get; set; }

    public string? FileName { get; set; }

    public string? FileType { get; set; }

    public byte[]? FileData { get; set; }
}
