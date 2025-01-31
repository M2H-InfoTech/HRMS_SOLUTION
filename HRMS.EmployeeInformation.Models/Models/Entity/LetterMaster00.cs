using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class LetterMaster00
{
    public int LetterTypeId { get; set; }

    public string? LetterCode { get; set; }

    public string? LetterTypeName { get; set; }

    public bool? Active { get; set; }
}
