using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models.Models.Entity;

public partial class SpecialWorkFlow02
{
    public long ValueId { get; set; }

    public int? TransactionId { get; set; }

    public int? LeaveType { get; set; }

    public int? WorkFlowId { get; set; }

    public int? LinkEmpId { get; set; }

    public int? LinkLevel { get; set; }

    public int? GrievanceTypeId { get; set; }

    public int? SurveyTopic { get; set; }

    public int? SurveyType { get; set; }

    public int? MainType { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
