using HRMS.EmployeeInformation.Models.Models.Entity;

namespace ATTENDANCE.DTO.Response
{
    public class ShiftInsertResultDto
    {
        public int ShiftId { get; set; }
        public List<HrShift01> Shift01List { get; set; }
    }
}
