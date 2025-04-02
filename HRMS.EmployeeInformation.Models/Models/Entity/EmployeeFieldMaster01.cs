using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class EmployeeFieldMaster01
{
    public int FieldMaster01Id { get; set; }

    public int? FieldMaster00Id { get; set; }

    public string? FieldCode { get; set; }

    public string? FieldDescription { get; set; }

    public int? Mandatory { get; set; }

    public int? Visibility { get; set; }

    public int? DataType { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
