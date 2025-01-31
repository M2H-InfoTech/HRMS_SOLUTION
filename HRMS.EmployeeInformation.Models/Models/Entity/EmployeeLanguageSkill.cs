using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class EmployeeLanguageSkill
{
    public int EmpLangId { get; set; }

    public int EmpId { get; set; }

    public string LanguageId { get; set; } = null!;

    public bool Read { get; set; }

    public bool Write { get; set; }

    public bool? Speak { get; set; }

    public bool? Comprehend { get; set; }

    public bool? MotherTongue { get; set; }

    public string? Status { get; set; }
}
