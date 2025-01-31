namespace MPLOYEE_INFORMATION.DTO.DTOs
{
    public class AddressDto
    {
        public int AddrID { get; set; }
        public int? EmpID { get; set; }
        public string? PermanentAddr { get; set; }
        public string? ContactAddr { get; set; }
        public string? PinNo1 { get; set; }
        public string? PinNo2 { get; set; }
        public int? CountryID1 { get; set; }
        public string? Country1 { get; set; }
        public int? CountryID2 { get; set; }
        public string? Country2 { get; set; }
        public string? Status { get; set; }
        public string? PhoneNo { get; set; }
        public string? AlterPhoneNo { get; set; }
        public string? MobileNo { get; set; }
    }
}
