using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class RewardAndRecognitionDto
    {
        public int RewardID { get; set; }
        public int? Emp_id { get; set; }
        public string? Achievement { get; set; }
        public string? RewardType { get; set; }
        public int RewardValue { get; set; }
        public string? Rewarddate { get; set; }
        public decimal? Amount { get; set; }
        public int? rewardidtype { get; set; }

        public string? Reason { get; set; }
    }
}


