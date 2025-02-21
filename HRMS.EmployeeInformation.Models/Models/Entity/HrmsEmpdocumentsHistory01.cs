using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.Models.Models.Entity;
public partial class HrmsEmpdocumentsHistory01
{
    public int DocFieldId { get; set; }

    public int? DocHisId { get; set; }

    public int? DetailId { get; set; }

    public int? DocFields { get; set; }

    public string? DocValues { get; set; }

    public string? OldDocValues { get; set; }
}

