using ATTENDANCE.Service.AssignPolicy;
using ATTENDANCE.Service.AttendancePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATTENDANCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancePolicyController(IAttendancePolicyService service) : ControllerBase
    {
        
    }
}
