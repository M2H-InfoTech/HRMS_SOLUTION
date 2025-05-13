namespace ATTENDANCE.DTO.Response.shift
{
    public class ShiftDto
    {
        public int ShiftId { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public string ShiftType { get; set; }
        public string EndwithNextDay { get; set; }
        public double? ToleranceForward { get; set; }
        public double? ToleranceBackward { get; set; }
    }

}
