namespace OFFICEKIT_CORE_ATTENDANCE.OFFICEKIT.Attendance.DTO.Response
{
    public class ShiftAccessDto
    {
        public long ShiftAccessID { get; set; }
        public int Emp_Id { get; set; } // Employee ID
        public string EmployeeName { get; set; } // Employee Name + Code
        public int? ShiftID { get; set; } // Shift ID
        public string ShiftName { get; set; } // Shift Name
        public string ShiftCodeName { get; set; } // Formatted Shift Code with timings
        public string WeekName { get; set; } // Weekend Name
        public int? EmployeeID { get; set; } // Employee ID reference
        public int? WeekEndMasterID { get; set; } // WeekEnd Master ID
        public string StartTime { get; set; } // Shift Start Time (converted to string for easier JSON serialization)
        public string EndTime { get; set; } // Shift End Time
        public string ValidDatefrom { get; set; } // Shift valid start date (formatted as dd/MM/yyyy)
        public string ValidDateTo { get; set; } // Shift valid end date (formatted as dd/MM/yyyy)
        public string Branch { get; set; } // Branch Name
        public string Designation { get; set; } // Designation Name
        public string ProjectName { get; set; } // Project Name
        public string ShiftStartEndTime { get; set; }
    }

}
