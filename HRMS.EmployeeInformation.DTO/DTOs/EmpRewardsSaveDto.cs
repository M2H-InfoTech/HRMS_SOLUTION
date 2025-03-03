using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class EmpRewardsSaveDto
    {
        public string? Achievement { get; set; }
        public int EmpId { get; set; }
        public int RewardType { get; set; }
        public string? Reason { get; set; }
        public decimal Amount { get; set; }
        public int DetailID { get; set; }

    }
}
