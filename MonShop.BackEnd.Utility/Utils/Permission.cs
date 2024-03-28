namespace MonShop.BackEnd.Utility.Utils;

public class Permission
{
    public const string ADMIN = "ADMIN";
    public const string STAFF = "STAFF";
    public const string USER = "USER";

    public const string ALL = $"{ADMIN}, {STAFF},{USER}";
    public const string MANAGEMENT = $"{ADMIN},{STAFF}";
}