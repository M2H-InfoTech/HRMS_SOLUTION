using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.MODDDD;

public partial class PayscaleCalculationValue
{
    public long PayscaleManualId { get; set; }

    public int? CalcType { get; set; }

    public int? BatchId { get; set; }

    public int? PayscaleRequest01Id { get; set; }

    public int? PaycodeId { get; set; }

    public int? ComponentType { get; set; }

    public int? FormulaId { get; set; }

    public int? RuleId { get; set; }

    public double? Amount { get; set; }

    public int? SortOrder { get; set; }

    public int? Type { get; set; }
}
