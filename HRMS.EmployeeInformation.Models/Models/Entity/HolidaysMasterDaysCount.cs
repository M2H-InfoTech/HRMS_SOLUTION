namespace HRMS.EmployeeInformation.Models.Models.Entity
{
    public partial class HolidaysMasterDaysCount
    {
        public int HolidayMasterDaysCountId { get; set; }

        public int? HolidayMasterId { get; set; }

        public int? DaysCount { get; set; }

        public int? InstId { get; set; }

        public DateTime? HolidayDate { get; set; }
    }
}
