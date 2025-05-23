using ATTENDANCE.DTO;
using AutoMapper;
using EMPLOYEE_INFORMATION.Data;
using Microsoft.EntityFrameworkCore;

namespace ATTENDANCE.Repository.ShiftMasterUpload
{
    public class ShiftMasterUploadRepository:IShiftMasterUploadRepository
    {
        private readonly IMapper _mapper;
        private readonly EmployeeDBContext _context;
        public ShiftMasterUploadRepository(IMapper mapper_, EmployeeDBContext context_)
        {
            _mapper = mapper_;
            _context = context_;
        }
        public async Task<List<MasterShiftUploadDetailsDto>> GetMasterShiftUploadDetails()
        {
            var data = await (from a in _context.HrShift00s
                              join b in _context.HrShift01s on a.ShiftId equals b.ShiftId into ab
                              from b in ab.DefaultIfEmpty( )
                              join c in _context.HrShift02s on a.ShiftId equals c.ShiftId into ac
                              from c in ac.DefaultIfEmpty( )
                              where a.IsUpload == "Y"
                              orderby a.EntryDate descending
                              select new MasterShiftUploadDetailsDto
                              {
                                  ShiftID = a.ShiftId,
                                  ShiftDate = b != null && b.EffectiveFrom.HasValue
                                    ? b.EffectiveFrom.Value.ToString("dd/MM/yyyy")
                                    : null,
                                  ShiftCode = a.ShiftCode,
                                  ShiftName = a.ShiftName,
                                  ShiftStartEnd = b != null
                            ? "(" + b.StartTime.ToString( ) + "-" + b.EndTime.ToString( ) + ")"
                            : null
                              }).ToListAsync( );
            return data;

        }
    }
}
