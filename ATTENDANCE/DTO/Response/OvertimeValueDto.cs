namespace ATTENDANCE.DTO.Response
{
    public class OvertimeValueDto
    {
        public int? Value { get; set; }
        public string Description { get; set; }
        public string Maximum { get; set; } = "0";
        public string Minimum { get; set; } = "0";
        public string WeekDays { get; set; } = "";
        public string Code { get; set; }
    }

}
