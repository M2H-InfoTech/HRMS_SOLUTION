using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class EmpProfessional
{
    public int ProdId { get; set; }

    public string? EmployeeCode { get; set; }

    public string? Company { get; set; }

    public string? CompanyAddress { get; set; }

    public string? Designation { get; set; }

    public string? PinZipCode { get; set; }

    public string? ContactPerson { get; set; }

    public string? ContactNumber { get; set; }

    public string? JobDescription { get; set; }

    public DateTime? JoiningDate { get; set; }

    public DateTime? RelievingDate { get; set; }

    public string? RelievingReason { get; set; }

    public string? AnnualCtc { get; set; }

    public string? Currency { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }
}
