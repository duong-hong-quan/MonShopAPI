namespace MonShop.BackEnd.Utility.Utils;

public class SD
{
    public static int MAX_RECORD_PER_PAGE = short.MaxValue;
    private static SD instance;

    private SD()
    {
    }

    public static SD getInstance()
    {
        if (instance == null) instance = new SD();
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

    public class SubjectMail
    {
        public static string VERIFY_ACCOUNT = "[THANK YOU] WELCOME TO MONSHOP. PLEASE VERIFY ACCOUNT";
        public static string WELCOME_TO_YOGA_CENTER = "[THANK YOU] WELCOME TO MONSHOP";
        public static string REMIND_PAYMENT = "REMIND PAYMENT";
        public static string PASSCODE_FORGOT_PASSWORD = "[MONSHOP] PASSCODE FORGOT PASSWORD";
    }

    public class ResponseMessage
    {
        public static string CREATE_SUCCESSFUL = "CREATE_SUCCESSFULLY";
        public static string UPDATE_SUCCESSFUL = "UPDATE_SUCCESSFULLY";
        public static string DELETE_SUCCESSFUL = "DELETE_SUCCESSFULLY";
        public static string CREATE_FAILED = "CREATE_FAILED";
        public static string UPDATE_FAILED = "UPDATE_FAILED";
        public static string DELETE_FAILED = "DELETE_FAILED";
        public static string LOGIN_FAILED = "LOGIN_FAILED";

        public static string NOTFOUND(object id, string entity)
        {
            return $"The {entity} with id {id} not found".ToUpper();
        }

        public static string NOTFOUND_BY_FIELDNAME(string fieldName, string entity)
        {
            return $"The {entity} with field name {fieldName} not found".ToUpper();
        }
    }

    public class FirebasePathName
    {
        public static string PRODUCT_PREFIX = "product/";
        public static string CATEGORY_PREFIX = "category/";
    }
}