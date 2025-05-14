using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class EmpCommunication
{
    public int ComId { get; set; }

    public string? EmployeeCode { get; set; }

    public string? PermanentAddress { get; set; }

    public string? Country { get; set; }

    public string? PinZipCode { get; set; }

    public string? ContactAddress { get; set; }

    public string? Country2 { get; set; }

    public string? PinZipCode2 { get; set; }

    public string? OfficePhone { get; set; }

    public string? PersonalPhone { get; set; }

    public string? MobileNo { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? HomeCountryPhone { get; set; }
}
