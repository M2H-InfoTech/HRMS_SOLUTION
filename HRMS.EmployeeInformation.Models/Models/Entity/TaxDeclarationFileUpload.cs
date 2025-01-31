using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class TaxDeclarationFileUpload
{
    public int InvstmntFileId { get; set; }

    public string? RequestCode { get; set; }

    public string? Remark { get; set; }

    public string? FlowStatus { get; set; }

    public string? UploadFileName { get; set; }

    public byte[]? FileImage { get; set; }

    public string? FileType { get; set; }

    public bool? Active { get; set; }

    public int? EmpId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
