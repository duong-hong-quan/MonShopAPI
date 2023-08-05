using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonShopLibrary.Utils
{
    public class Util
    {

        private static Util Instance;
        private Util() { }
        public static Util getInstance()
        {
            if (Instance == null)
            {
                Instance = new Util();
            }
            return Instance;
        }
        public DateTime GetCurrentDateTimeInTimeZone()
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Lấy thời gian hiện tại theo múi giờ địa phương của máy tính
            DateTime localTime = DateTime.Now;

            // Chuyển đổi thời gian địa phương sang múi giờ của Việt Nam
            DateTime vietnamTime = TimeZoneInfo.ConvertTime(localTime, vietnamTimeZone);

            return vietnamTime;
        }

        public DateTime GetCurrentDateInTimeZone()
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Lấy thời gian hiện tại theo múi giờ địa phương của máy tính
            DateTime localTime = DateTime.Now;

            // Chuyển đổi thời gian địa phương sang múi giờ của Việt Nam
            DateTime vietnamTime = TimeZoneInfo.ConvertTime(localTime, vietnamTimeZone);

            return vietnamTime.Date;
        }

        private static HashSet<int> generatedNumbers = new HashSet<int>();

        public static int GenerateUniqueNumber()
        {
            while (true)
            {
                // Lấy thời gian hiện tại dưới dạng timestamp
                long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

                // Tạo số ngẫu nhiên
                Random random = new Random();
                int randomNumber = random.Next(1000, 10000);

                // Kết hợp thời gian và số ngẫu nhiên để tạo số nguyên dương
                int uniqueNumber = (int)(timestamp + randomNumber);

                // Kiểm tra xem số đã tồn tại chưa
                if (!generatedNumbers.Contains(uniqueNumber))
                {
                    generatedNumbers.Add(uniqueNumber);
                    return uniqueNumber;
                }
            }
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
            {
                if (sectionObject.ContainsKey(key))
                {
                    sectionObject[key] = value;
                    var updatedJson = JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(appSettingsPath, updatedJson);
                }
            }
        }


    }
}
