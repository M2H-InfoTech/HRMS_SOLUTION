using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class HrEmpAddress01
{
    public int AddrId { get; set; }

    public int? EmpId { get; set; }

    public string? PermanentAddr { get; set; }

    public string? PinNo1 { get; set; }

    public int? Addr1Country { get; set; }

    public string? ContactAddr { get; set; }

    public string? PinNo2 { get; set; }

    public int? Addr2Country { get; set; }

    public int? ApprlId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? PhoneNo { get; set; }

    public string? AlterPhoneNo { get; set; }

    public string? MobileNo { get; set; }
}
