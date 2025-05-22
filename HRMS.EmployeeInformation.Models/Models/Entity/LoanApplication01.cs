using System;
using System.Collections.Generic;

namespace HRMS.EmployeeInformation.Models;

public partial class LoanApplication01
{
    public long LoanChild01Id { get; set; }

    public int? AssignLoanId { get; set; }

    public int? EmployeeId { get; set; }

    public double? LoanTotalAmount { get; set; }

    public double? BalanceLoanAmount { get; set; }

    public string? LoanSettlementStatus { get; set; }
}
