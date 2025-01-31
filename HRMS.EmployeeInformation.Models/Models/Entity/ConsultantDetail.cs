using System;
using System.Collections.Generic;

namespace EMPLOYEE_INFORMATION.Models;

public partial class ConsultantDetail
{
    public int Id { get; set; }

    public string? ConsultantName { get; set; }

    public string? PhoneNo { get; set; }

    public string? MobileNumber { get; set; }

    public string? Address { get; set; }
}
