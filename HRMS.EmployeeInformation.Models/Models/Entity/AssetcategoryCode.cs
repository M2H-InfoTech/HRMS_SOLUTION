using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class AssetcategoryCode
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public int? Value { get; set; }

    public DateTime? Createdby { get; set; }

    public string? Status { get; set; }

    public string? Assetclass { get; set; }

    public string? AssetModel { get; set; }
}
