using System;
using System.Collections.Generic;


namespace EMPLOYEE_INFORMATION.Models.Entity;

public partial class HrEmpEmergaddressApprl
{
    public int AddrId { get; set; }

    public int? EmpId { get; set; }

    public string? Address { get; set; }

    public string? PinNo { get; set; }

    public int? Country { get; set; }

    public string? PhoneNo { get; set; }

    public string? AlterPhoneNo { get; set; }

    public string? MobileNo { get; set; }

    public string? RequestId { get; set; }

    public string? Status { get; set; }

    public string? FlowStatus { get; set; }

    public DateTime? DateFrom { get; set; }

    public int? MasterId { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
