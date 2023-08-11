using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShopLibrary.DTO;
using MonShopLibrary.Utils;
using Newtonsoft.Json.Linq;

namespace MonShopAPI.Controller
{
    [Route("System")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        [Route("GetAppSettings.json")]
        [HttpGet]
        public IActionResult GetAppSettings()
        {
            var appSettings = Utility.getInstance().ReadAppSettingsJson();
            var formattedAppSettings = JToken.Parse(appSettings).ToString(Newtonsoft.Json.Formatting.Indented);
            return Content(formattedAppSettings, "application/json");
        }


        [Route("UpdateAppSetting.json")]

        [HttpPost]
        public IActionResult UpdateAppSetting([FromBody] AppSettingDTO dto)
        {
            Utility.getInstance().UpdateAppSettingValue(dto.Section, dto.Key, dto.Value);
            return Ok();
        }
    }
}
