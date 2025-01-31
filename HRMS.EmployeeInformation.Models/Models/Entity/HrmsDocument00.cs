using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class HrmsDocument00
{
    public long DocId { get; set; }

    public string? DocName { get; set; }

    public int? DocType { get; set; }

    public bool? IsMandatory { get; set; }

    public bool? Active { get; set; }

    public int? IsExpiry { get; set; }

    public int? NotificationCountDays { get; set; }

    public string? FolderName { get; set; }

    public int? IsAllowMultiple { get; set; }

    public int? IsEsi { get; set; }

    public int? IsPf { get; set; }

    public int? ShowInRecruitment { get; set; }
}
