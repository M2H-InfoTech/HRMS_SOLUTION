using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
   public class GetEditbasicsettingsdto
    {
        public string SettingsName { get; set; }
        public string SettingsDescription { get; set; }
        public int? LeaveMasterId { get; set; }
        public int? DaysOrHours { get; set; }

    }
}
