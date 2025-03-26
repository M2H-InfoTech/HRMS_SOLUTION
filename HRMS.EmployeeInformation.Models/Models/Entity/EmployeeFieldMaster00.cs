using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class EmployeeFieldMaster00
{
    public int FieldMaster00Id { get; set; }

    public string? FieldCode { get; set; }

    public string? FieldDescription { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
