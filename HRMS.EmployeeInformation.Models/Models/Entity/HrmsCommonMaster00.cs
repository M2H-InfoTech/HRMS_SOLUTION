using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrmsCommonMaster00
{
    public int ComMasId { get; set; }

    public int? ComId { get; set; }

    public int? ComTypeId { get; set; }

    public string? AssetType { get; set; }

    public bool? SepActive { get; set; }

    public int? IdentificationId { get; set; }

    public string? Status { get; set; }
}
