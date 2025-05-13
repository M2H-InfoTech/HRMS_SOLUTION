using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class HrShiftOpen
{
    public int OpenShiftId { get; set; }

    public int? ShiftMasterId { get; set; }

    public int? ShiftId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
