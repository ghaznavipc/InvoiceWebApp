namespace Common.Utilities;

public static class MobileHelper
{
    public static string HideMobileNumber(this string mobile)
    {
        return string.Concat(mobile.AsSpan(0, 4), "---", mobile.AsSpan(7, 4));
    }
}
