using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class AdmCodegenerationmaster
{
    public int CodeId { get; set; }

    public int? InstId { get; set; }

    public int? TypeId { get; set; }

    public string? Code { get; set; }

    public int? Value { get; set; }

    public int? IncrementCount { get; set; }

    public bool? Manual { get; set; }

    public int? CurrentCodeValue { get; set; }

    public string? NumberFormat { get; set; }

    public string? LastSequence { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? MnodifiedDate { get; set; }

    public int? CompanyTransferEmpCode { get; set; }

    public int? PrefixFormatId { get; set; }

    public int? SuffixFormatId { get; set; }

    public string? Suffix { get; set; }

    public string? FinalCodeWithNoSeq { get; set; }

    public int? IsDelete { get; set; }
}
