using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class EmployeeHobby
{
    public int Hid { get; set; }

    public string? HobbieId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? EntryDate { get; set; }
}
