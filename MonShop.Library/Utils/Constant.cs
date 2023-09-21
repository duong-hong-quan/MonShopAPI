using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.Utils
{
    public class Constant
    {

        private Constant() { }
        private static Constant instance;

        public static Constant getInstance()
        {
            if (instance == null)
            {
                instance = new Constant();
            }
            return instance;
        }
        public class Order
        {
            public static int PENDING_PAY = 1;
            public static int SUCCESS_PAY = 2;
            public static int FAILURE_PAY = 3;
            public static int SHIPPED = 4;
            public static int DELIVERED = 5;
            public static int CANCELLED = 6;
            

        }

        public class Category
        {
            public static int PANTS = 1;
            public static int SHIRTS = 2;
            public static int SHOES = 3;
            public static int ACCESSORIES = 4;


        }

        public class Product
        {
            public static int ACTIVE = 1;
            public static int IN_ACTIVE = 2;

        }

        public class PaymentType
        {
            public static int PAYMENT_MOMO = 1;
            public static int PAYMENT_VNPAY = 2;
            public static int PAYMENT_PAYPAL = 3;


        }
    }
}
