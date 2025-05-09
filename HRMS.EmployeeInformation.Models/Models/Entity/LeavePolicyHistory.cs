using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class LeavePolicyHistory
{
    public long LeavePolicyHistoryId { get; set; }

    public int? Leavepolicymasterid { get; set; }

    public int? EmployeeId { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? Ipaddress { get; set; }
}
