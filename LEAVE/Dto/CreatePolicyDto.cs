namespace LEAVE.Dto
{
    public class CreatePolicyDto
    {
        public int LeavePolicyMasterID { get; set; }
        public int Inst_Id { get; set; }
        public string? Name { get; set; }
        public int BlockMultiUnapprovedLeaves { get; set; }
        public int EntryBy { get; set; }
        public int EmpId { get; set; }
    }
}
