using System;
using System.Collections.Generic;

namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.Models;

public partial class EntityApplicable00
{
    public long ApplicableId { get; set; }

    public long? TransactionId { get; set; }

    public long? LinkLevel { get; set; }

    public long? LinkId { get; set; }

    public long? MasterId { get; set; }

    public long? MainMasterId { get; set; }

    public long? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
