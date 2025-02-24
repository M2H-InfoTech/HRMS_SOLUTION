using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class Dependent01
{
    public int DocId { get; set; }

    public int? DepId { get; set; }

    public int? DepEmpid { get; set; }

    public string? DocFileType { get; set; }

    public string? DocFileName { get; set; }

    public byte[]? FileData { get; set; }
}
