using ATTENDANCE.Service.ShiftUpload;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATTENDANCE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftUploadController(IShiftUploadService service) : ControllerBase
    {
    }
}
