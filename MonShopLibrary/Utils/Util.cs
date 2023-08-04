using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.Utils
{
    public class Util
    {

        private static Util instance;
        private Util() { }
        public static Util getInstance()
        {
            if (instance == null)
            {
                instance = new Util();
            }
            return instance;
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

    }
}
