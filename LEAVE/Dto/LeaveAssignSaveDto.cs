using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEAVE.Dto
{
    public class LeaveAssignSaveDto
    {
        public string LeaveMasters { get; set; }
        public string EmployeeIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ValidTo { get; set; }
        public int EntryBy { get; set; }
        public int LinkLevel { get; set; }
        public int LevelId { get; set; }
    }
}
