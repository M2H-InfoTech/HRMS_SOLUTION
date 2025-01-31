using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class HrEmpImage
{
    public int InstId { get; set; }

    public int EmpImgId { get; set; }

    public int EmpId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string Active { get; set; } = null!;

    public string? FingerUrl { get; set; }

    public string? EmpImage { get; set; }
}
