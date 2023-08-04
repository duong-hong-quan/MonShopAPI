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

        }
    }
}
