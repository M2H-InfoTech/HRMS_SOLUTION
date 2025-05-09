using System;
using System.Collections.Generic;
namespace HRMS.EmployeeInformation.Models.Models.Entity
{

    public partial class LeavePolicyLeaveInclude
    {
        public int LeaveIncludeId { get; set; }

        public int? LeavePolicyInstanceLimitId { get; set; }

        public int? Leavestatus { get; set; }

        public decimal? Fromdays { get; set; }

        public int? Leavetype { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Createddate { get; set; }

        public decimal? LeaveDays { get; set; }

        public int? OffdaysIncExc { get; set; }
    }
}