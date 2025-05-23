using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class TmpRewardInsertDto
    {
        public string EmployeeCode { get; set; }
        [JsonProperty("RewardDate")]
        public DateTime? RewardDate { get; set; }
        public string Achievement { get; set; }
        public string RewardType { get; set; }
        public decimal? Amount { get; set; }
        public string? RewardsReason { get; set; }
    }

}
