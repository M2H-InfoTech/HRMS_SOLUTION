namespace ATTENDANCE.DTO.Request
{
    public class ShortageDto
    {
        public int ShortageId { get; set; }
        public double? PercentageFrom { get; set; }
        public double? PercentageTo { get; set; }
    }

}
