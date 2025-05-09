namespace LEAVE.Dto
{
    public class LeavePolicyInstanceLimitDto
    {

        public int LeavePolicyMasterID { get; set; }
        public int LeavePolicyInstanceLimitID { get; set; }
        public int Inst_Id { get; set; }
        public int LeaveID { get; set; }
        public float MaximamLimit { get; set; }
        public float MinimumLimit { get; set; }
        public bool IsHolidayIncluded { get; set; }
        public bool IsWeekendIncluded { get; set; }
        public decimal NoOfDayIncludeHoliday { get; set; }
        public decimal NoOfDayIncludeWeekEnd { get; set; }
        public int EntryBy { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        public decimal Daysbtwnleaves { get; set; }
        public decimal Salaryadvancedays { get; set; }
        public decimal Roledeligationdays { get; set; }
        public decimal Attachmentdays { get; set; }
        public decimal ProbationML { get; set; }
        public decimal NewjoinML { get; set; }
        public decimal OtherML { get; set; }
        public int Halfday { get; set; }
        public int PredatedApplication { get; set; }
        public decimal Daysbtwndifferentleave { get; set; }
        public decimal Daysleaveclubbing { get; set; }
        public decimal Predateddayslimit { get; set; }
        public int Returndate { get; set; }
        public int Autotravelapprove { get; set; }
        public int Leaveinclude { get; set; }
        public int Contactdetails { get; set; }
        public int Leavereason { get; set; }
        public int Approvremark { get; set; }
        public int Nobalance { get; set; }
        public int Applyafterallleave { get; set; }
        public string Applyafterleaveids { get; set; }
        public int Showinapplicationonly { get; set; }
        public int Rejectremark { get; set; }
        public int Predatedapplicationproxy { get; set; }
        public float Predateddayslimitproxy { get; set; }
        public int PredatedapplicationAttendance { get; set; }
        public float PredatedapplicationAttendanceDays { get; set; }
        public int FutureleaveApplication { get; set; }
        public float FutureleaveApplicationDays { get; set; }
    }
}
