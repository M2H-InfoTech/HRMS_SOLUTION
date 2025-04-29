using EMPLOYEE_INFORMATION.Data;
using HRMS.EmployeeInformation.DTO.DTOs;
using HRMS.EmployeeInformation.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LEAVE.Repository.BasicSettings
{
    public class BasicSettingsRepository : IBasicSettingsRepository
    {
        private readonly EmployeeDBContext _context;
        private readonly HttpClient _httpClient;
        public BasicSettingsRepository(EmployeeDBContext dbContext, HttpClient httpClient)
        {
            _context = dbContext;
            _httpClient = httpClient;
        }

        public async Task<List<FillvacationaccrualDto>> Fillvacationaccrual(int basicsettingsid)
        {
            var result = await (from a in _context.LeaveScheme00s
                                join b in _context.Leavescheme02s
                                    on a.LeaveSchemeId equals b.LeaveSchemeId
                                where b.Basicsettingsid == basicsettingsid
                                select new FillvacationaccrualDto
                                {
                                    VacationSchemeId = a.LeaveSchemeId,
                                    Scheme = a.SchemeDescription + " [ " + a.SchemeCode + " ]"
                                }).ToListAsync();

            return result;
        }


        public async Task<List<GetEditbasicsettingsdto>> GetEditbasicsettings(int basicsettingsid)
        {
            var result = from a in _context.HrmLeaveBasicSettings
                         join b in _context.HrmLeaveMasterandsettingsLinks
                             on a.SettingsId equals b.SettingsId
                         where a.SettingsId == basicsettingsid
                         select new GetEditbasicsettingsdto
                         {
                             SettingsName = a.SettingsName,
                             SettingsDescription = a.SettingsDescription,
                             LeaveMasterId = b.LeaveMasterId,
                             DaysOrHours = a.DaysOrHours
                         };

            return await result.ToListAsync();

        }

        public Task<List<GetEditbasicsettingsdto>> saveleavelinktable(int Masterid)
        {
            throw new NotImplementedException();
        }

        public async Task<int?> DeleteConfirm(int basicSettingsId)
        {
            bool exists = await _context.HrmLeaveBasicsettingsaccesses
                .AnyAsync(x => x.SettingsId == basicSettingsId);

            if (exists)
                return -1;

            return 1; // or null or 0 if no restriction, based on your logic
        }


        public async Task<int?> GetDeletebasics(int basicSettingsId, int masterId)
        {

            var transactionIdTask = _httpClient.GetAsync("http://localhost:5194/gateway/Employee/GetTransactionIdByTransactionType?transactionType=Leave_BS");
            await Task.WhenAll(transactionIdTask);

            // Parse the results
            var transIdString = await transactionIdTask.Result.Content.ReadAsStringAsync();
            if (!int.TryParse(transIdString, out int transactionId))
            {
                throw new InvalidOperationException("Failed to parse transaction ID.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (masterId == 0)
                {
                    // Delete from HRM_LEAVE_BASIC_SETTINGS
                    var basicSettings = await _context.HrmLeaveBasicSettings
                        .Where(x => x.SettingsId == basicSettingsId)
                        .ToListAsync();
                    _context.HrmLeaveBasicSettings.RemoveRange(basicSettings);

                   

                    // Delete from EntityApplicable00
                    var applicableEntities = await _context.EntityApplicable00s
                        .Where(e => e.MasterId == basicSettingsId && e.TransactionId == transactionId)
                        .ToListAsync();
                    _context.EntityApplicable00s.RemoveRange(applicableEntities);

                    // Delete from HRM_LEAVE_MASTERANDSETTINGS_LINK
                    var masterLinks = await _context.HrmLeaveMasterandsettingsLinks
                        .Where(link => link.SettingsId == basicSettingsId)
                        .ToListAsync();
                    _context.HrmLeaveMasterandsettingsLinks.RemoveRange(masterLinks);

                    // Get SettingsDetailsId (head)
                    var detail = await _context.HrmLeaveBasicsettingsDetails
                        .Where(d => d.SettingsId == basicSettingsId)
                        .FirstOrDefaultAsync();

                    if (detail != null)
                    {
                        int headId = detail.SettingsDetailsId;

                        // Delete from HRM_LEAVE_BASICSETTINGS_DETAILS
                        _context.HrmLeaveBasicsettingsDetails.Remove(detail);

                        // Delete from HRM_LEAVE_EXCEPTIONAL_ELIGIBILITY
                        var eligibility = await _context.HrmLeaveExceptionalEligibilities
                            .Where(e => e.SettingsDetailsHeadId == headId)
                            .ToListAsync();
                        _context.HrmLeaveExceptionalEligibilities.RemoveRange(eligibility);
                    }

                    // Get LeaveEntitlementId
                    var entitlement = await _context.HrmLeaveEntitlementHeads
                        .Where(e => e.SettingsId == basicSettingsId)
                        .FirstOrDefaultAsync();

                    if (entitlement != null)
                    {
                        int entId = entitlement.LeaveEntitlementId;

                        _context.HrmLeaveEntitlementHeads.Remove(entitlement);

                        var regs = await _context.HrmLeaveEntitlementRegs
                            .Where(r => r.LeaveEntitlementId == entId)
                            .ToListAsync();
                        _context.HrmLeaveEntitlementRegs.RemoveRange(regs);

                        var serviceLeaves = await _context.HrmLeaveServicedbasedleaves
                            .Where(s => s.LeaveEntitlementId == entId)
                            .ToListAsync();
                        _context.HrmLeaveServicedbasedleaves.RemoveRange(serviceLeaves);

                        var partials = await _context.HrmLeavePartialPayments
                            .Where(p => p.SettingsDetailsId == entId)
                            .ToListAsync();
                        _context.HrmLeavePartialPayments.RemoveRange(partials);
                    }
                }
                else
                {
                    // Delete single mapping if exists
                    var specificLink = await _context.HrmLeaveMasterandsettingsLinks
                        .Where(link => link.SettingsId == basicSettingsId && link.LeaveMasterId == masterId)
                        .ToListAsync();
                    _context.HrmLeaveMasterandsettingsLinks.RemoveRange(specificLink);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return basicSettingsId;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
