using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Common.Utilities;

public static class StringExtensions
{
	public static bool HasValue([NotNullWhen(true)] this string? value, bool ignoreWhiteSpace = true)
    {
        return ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
    }

	public static int ToInt(this string value)
    {
        return Convert.ToInt32(value);
    }

    /// <summary>will check if all characters are English Number</summary>
    public static bool IsAllDigit(this string value)
    {
        return Regex.IsMatch(value, @"^\d+$");
    }


    /// <summary>will check if value is somewhat valid to be an Iranian national code</summary>
    public static bool IsNationalCode(this string input)
    {
        if (input.Length is < 8 or > 10 && input.IsAllDigit())
            return false;

        if (input.Length == 8) input = '0' + input;
        if (input.Length == 9) input = '0' + input;

        string[] allEqual = { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
        if (allEqual.Any(s => s == input))
            return false;

        int check = Convert.ToInt32(input.Substring(9, 1));
        int sum = Enumerable.Range(0, 9)
            .Select(x => Convert.ToInt32(input.Substring(x, 1)) * (10 - x))
            .Sum() % 11;

        return sum < 2 ? check == sum : check + sum == 11;
    }


    /// <summary>remove last N char from string</summary>
    /// <param name="count">Number of char to delete from the last</param>
    /// <returns>remaining value (empty if null or count > Length)</returns>
    public static string RemoveLast(this string str, int count = 1)
    {
        if (!str.HasValue(false) || count > str.Length)
            return string.Empty;

        return str.Remove(str.Length - count);
    }

    public static decimal ToLong(this string value)
	{
		return Convert.ToInt64(value);
	}

	public static decimal ToDecimal(this string value)
    {
        return Convert.ToDecimal(value);
    }

    public static string ToNumeric(this int value)
    {
        return value.ToString("N0"); //"123,456"
    }

	public static string ToNumeric(this long value)
	{
		return value.ToString("N0");
	}

	public static string ToNumeric(this decimal value)
    {
        return value.ToString("N0");
    }

    public static string ToCurrency(this int value)
    {
        //fa-IR => current culture currency symbol => ریال
        //123456 => "123,123ریال"
        return value.ToString("C0");
    }

	public static string ToCurrency(this long value)
	{
		return value.ToString("C0");
	}

	public static string ToCurrency(this decimal value)
    {
        return value.ToString("C0");
    }

    public static string En2Fa(this string str)
    {
        return str.Replace("0", "۰")
				  .Replace("1", "۱")
				  .Replace("2", "۲")
				  .Replace("3", "۳")
				  .Replace("4", "۴")
				  .Replace("5", "۵")
				  .Replace("6", "۶")
				  .Replace("7", "۷")
				  .Replace("8", "۸")
				  .Replace("9", "۹");
    }

    public static string Fa2En(this string str)
    {
        return str.Replace("۰", "0")
				  .Replace("۱", "1")
				  .Replace("۲", "2")
				  .Replace("۳", "3")
				  .Replace("۴", "4")
				  .Replace("۵", "5")
				  .Replace("۶", "6")
				  .Replace("۷", "7")
				  .Replace("۸", "8")
				  .Replace("۹", "9")
				  .Replace("٠", "0") //iphone numeric
				  .Replace("١", "1")
				  .Replace("٢", "2")
				  .Replace("٣", "3")
				  .Replace("٤", "4")
				  .Replace("٥", "5")
				  .Replace("٦", "6")
				  .Replace("٧", "7")
				  .Replace("٨", "8")
				  .Replace("٩", "9");
    }

    public static string FixPersianChars(this string str)
    {
        return str.Replace("ﮎ", "ک")
				  .Replace("ﮏ", "ک")
				  .Replace("ﮐ", "ک")
				  .Replace("ﮑ", "ک")
				  .Replace("ك", "ک")
				  .Replace("ي", "ی")
				  .Replace(" ", " ")
				  .Replace("‌" , " ")
				  .Replace("ھ", "ه");//.Replace("ئ", "ی");
    }

    public static string CleanString(this string? str)
    {
        return str?.Trim().FixPersianChars().Fa2En() ?? string.Empty;
    }

    public static string? NullIfEmpty(this string? str) => str?.Length == 0 ? null : str;

    public static string EmptyIfNull(this string? str) => str ?? string.Empty;

    public static string NormalizeTime(this string? time)
    {
		return time?.Length switch
		{
			11 => string.Concat(time.AsSpan(8, 1), ":", time.AsSpan(9, 2)),
			_  => string.Concat(time.AsSpan(8, 2), ":", time.AsSpan(10, 2))
		};
	}
}
