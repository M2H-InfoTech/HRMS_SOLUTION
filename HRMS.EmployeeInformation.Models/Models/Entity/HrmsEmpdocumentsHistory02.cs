using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.Models.Models.Entity;
public partial class HrmsEmpdocumentsHistory02
{
    public int DocAttachId { get; set; }

    public int? DocHisId { get; set; }

    public int? DetailId { get; set; }

    public string? FileName { get; set; }

    public string? FileType { get; set; }

    public byte[]? FileData { get; set; }

    public string? Status { get; set; }

    public string? OldFileName { get; set; }
}
