using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class PayCodeMaster01
{
    public int PayCodeId { get; set; }

    public string PayCode { get; set; } = null!;

    public string PayCodeDescription { get; set; } = null!;

    public int Type { get; set; }

    public bool NonPayableAllowance { get; set; }

    public int? ApplicableType { get; set; }

    public int? ApplicableDailyRate { get; set; }

    public int? PayCodeMasterId { get; set; }

    public int? VarAmount { get; set; }

    public string? SortOrder { get; set; }

    public string? ApplicableValue { get; set; }

    public string? ApplicableEsipf { get; set; }

    public string? Code { get; set; }

    public int? VisibilityCode { get; set; }

    public int? PayRollReportId { get; set; }

    public string? CompPercentage { get; set; }

    public int? Wpsorder { get; set; }

    public int? EnableInTemporaryComponents { get; set; }
}
