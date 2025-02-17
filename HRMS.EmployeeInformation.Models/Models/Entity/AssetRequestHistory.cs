using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class AssetRequestHistory
{
    public int AsstHisId { get; set; }

    public int? RequestId { get; set; }

    public int? StatusType { get; set; }

    public int? AssetEntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public int? AssetEmpId { get; set; }
}
