using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.Models.Models.Entity
{
    public partial class LeaveScheme00
    {
        public int LeaveSchemeId { get; set; }

        public string? SchemeCode { get; set; }

        public string? SchemeDescription { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }

}
