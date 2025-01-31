using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrEmpAddress01Apprl
{
    public int AddrId { get; set; }

    public int? EmpId { get; set; }

    public string? PermanentAddr { get; set; }

    public string? PinNo1 { get; set; }

    public int? Addr1Country { get; set; }

    public string? ContactAddr { get; set; }

    public string? PinNo2 { get; set; }

    public int? Addr2Country { get; set; }

    public string? RequestId { get; set; }

    public string? Status { get; set; }

    public string? FlowStatus { get; set; }

    public DateTime? DateFrom { get; set; }

    public int? MasterId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? PhoneNo { get; set; }

    public string? AlterPhoneNo { get; set; }

    public string? MobileNo { get; set; }
}
