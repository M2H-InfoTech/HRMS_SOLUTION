using LEAVE.Service.BasicSettings;
using Microsoft.AspNetCore.Mvc;

namespace LEAVE.Controllers.BasicSettings
{
    public class BasicSettingsController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly IBasicSettingsService _basicSettingsService;
        public BasicSettingsController(IBasicSettingsService basicSettingsService)
        {

            _basicSettingsService = basicSettingsService;
        }

        [HttpGet("Fillvacationaccrual")]
        public async Task<IActionResult> Fillvacationaccrual(int basicsettingsid)
        {
            var fillvacationaccrual = await _basicSettingsService.Fillvacationaccrual(basicsettingsid);
            return Ok(fillvacationaccrual);
        }
        [HttpGet("GetEditbasicsettings")]
        public async Task<IActionResult> GetEditbasicsettings(int Masterid)
        {
            var GetEditbasicsettings = await _basicSettingsService.GetEditbasicsettings(Masterid);
            return Ok(GetEditbasicsettings);
        }
        [HttpGet("saveleavelinktable")]
        public async Task<IActionResult> saveleavelinktable(int Masterid)
        {
            var saveleavelinktable = await _basicSettingsService.saveleavelinktable(Masterid);
            return Ok(saveleavelinktable);
        }
    }
}
