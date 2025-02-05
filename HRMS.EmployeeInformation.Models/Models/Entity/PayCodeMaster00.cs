using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class PayCodeMaster00
{
    public int PayCodeMasterId { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? CurrencyId { get; set; }

    public int? CreatedBy { get; set; }

    public int? PayrollReportFormat { get; set; }

    public int? DefaultRoundoff { get; set; }

    public int? RoundTotalAmountOnly { get; set; }

    public int? Arrearsformat { get; set; }

    public int? Wpsformat { get; set; }

    public bool? ShowGratituty { get; set; }

    public int? EmailFsSheet { get; set; }

    public int? SatuatoryLinkFormat { get; set; }

    public int? GridDisplayFormat { get; set; }

    public int? PaySlipFormat { get; set; }
}
