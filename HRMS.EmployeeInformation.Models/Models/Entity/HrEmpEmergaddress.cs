using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class HrEmpEmergaddress
{
    public int AddrId { get; set; }

    public int? EmpId { get; set; }

    public string? Address { get; set; }

    public string? PinNo { get; set; }

    public int? Country { get; set; }

    public string? PhoneNo { get; set; }

    public string? AlterPhoneNo { get; set; }

    public string? MobileNo { get; set; }

    public int? ApprlId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? EmerName { get; set; }

    public string? EmerRelation { get; set; }
}
