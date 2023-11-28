using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.Utility;
using Newtonsoft.Json.Linq;

namespace MonShop.BackEnd.API.Controller
{
    [Route("System")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        [Route("GetAppSettings.json")]
        [HttpGet]
        public IActionResult GetAppSettings()
        {
            var appSettings = Utility.Utils.Utility.GetInstance().ReadAppSettingsJson();
            var formattedAppSettings = JToken.Parse(appSettings).ToString(Newtonsoft.Json.Formatting.Indented);
            return Content(formattedAppSettings, "application/json");
        }


        [Route("UpdateAppSetting.json")]

        [HttpPost]
        public IActionResult UpdateAppSetting([FromBody] AppSettingDTO dto)
        {
            Utility.Utils.Utility.GetInstance().UpdateAppSettingValue(dto.Section, dto.Key, dto.Value);
            return Ok();
        }
    }
}
