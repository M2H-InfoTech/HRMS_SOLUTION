using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class DeletedSavedEmployeeHistory
{
    public int Id { get; set; }

    public int? EmpId { get; set; }

    public string? Comments { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
