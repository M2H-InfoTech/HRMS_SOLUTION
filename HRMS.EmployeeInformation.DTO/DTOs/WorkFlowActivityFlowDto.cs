using HRMS.EmployeeInformation.Models.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.EmployeeInformation.DTO.DTOs
{
    public class WorkFlowActivityFlowDto
    {
        public int EmpId {  get; set; }
        public string? TransactionType { get; set; }
        public int ReturnWorkFlowTable {  get; set; }
        public int SpecialWorkFlowID { get; set; }
        public int SpecialWorkFlowSubID { get; set; }
        public int RequestID { get; set; }
        public int GrievanceWistleBlower { get; set; }
        public DateTime? CommonDate { get; set; }
        public string? ApprovalRemarks {  get; set; }
        public int EntryBy {  get; set; }
        public string? Deligate { get; set; }
        public string? Entryfrom { get;set; }
        public int? ReqId { get; set; }
        public int? DubEmpID { get; set; }
        public string Surveytopic { get;set; }
        public int? loanTypeId { get;set; }

    }

    public class WorkFlowDisplayDto
    {
        public string? EmpName { get; set; }
        public string? Name { get; set; }
        public string Url { get; set; }
        public string? Designation { get; set; }
        public string ImageUrl { get;set; }
        public string? ApprovalStatus { get;set; }
        public string? Branch { get; set; }
        public int? Rule { get; set; }
        public int RuleOrder { get; set; }
        public bool ShowStatus { get; set; }
        public int FlowRoleID { get; set; }
        public int ForwardNext { get; set; }
        public int WorkFlowID { get; set; }
        public string WorkFlowStatus {  get; set; }
        public int EmpId { get; set; }
        public string? Code { get; set; }

    }
}
