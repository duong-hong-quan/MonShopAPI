using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonShop.BackEnd.Utility.Utils;

public class Utility
{
    private static Utility Instance;

    private Utility()
    {
    }

    public static Utility GetInstance()
    {
        if (Instance == null) Instance = new Utility();
        return Instance;
    }

    public DateTime GetCurrentDateTimeInTimeZone()
    {
        var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        // Lấy thời gian hiện tại theo múi giờ địa phương của máy tính
        var localTime = DateTime.Now;

        // Chuyển đổi thời gian địa phương sang múi giờ của Việt Nam
        var vietnamTime = TimeZoneInfo.ConvertTime(localTime, vietnamTimeZone);

        return vietnamTime;
    }

    public DateTime GetCurrentDateInTimeZone()
    {
        var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        // Lấy thời gian hiện tại theo múi giờ địa phương của máy tính
        var localTime = DateTime.Now;

        // Chuyển đổi thời gian địa phương sang múi giờ của Việt Nam
        var vietnamTime = TimeZoneInfo.ConvertTime(localTime, vietnamTimeZone);

        return vietnamTime.Date;
    }

    public string ReadAppSettingsJson()
    {
        var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        return File.ReadAllText(appSettingsPath);
    }

    public void UpdateAppSettingValue(string section, string key, string value)
    {
        var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        var json = File.ReadAllText(appSettingsPath);
        var settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        if (settings.ContainsKey(section) && settings[section] is JObject sectionObject)
            if (sectionObject.ContainsKey(key))
            {
                sectionObject[key] = value;
                var updatedJson = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(appSettingsPath, updatedJson);
            }
    }
}