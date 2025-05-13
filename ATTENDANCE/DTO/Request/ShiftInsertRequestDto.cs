using ATTENDANCE.DTO.Response;
using ATTENDANCE.DTO.Response.shift;

namespace ATTENDANCE.DTO.Request
{
    public class ShiftInsertRequestDto
    {
        public int? ShiftID { get; set; }
        public string? ShiftCode { get; set; }
        public int? CompanyID { get; set; }
        public string? ShiftName { get; set; }
        public string? ShiftType { get; set; }
        public string? EndwithNextDay { get; set; }
        public int? EntryBy { get; set; }
        public DateOnly? EntryDate { get; set; }
        public double? ToleranceForward { get; set; }
        public double? ToleranceBackward { get; set; }

        public List<ShiftTime>? TypeShiftTimeList { get; set; }
        public List<BreakTimeDto>? BreakTimeList { get; set; }
        public List<OpenShiftTable>? ShiftTimeList { get; set; }
    }
}
