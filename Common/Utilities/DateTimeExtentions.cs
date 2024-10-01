using System.Globalization;

namespace Common.Utilities;

public static class DateTimeExtentions
{
	/// <summary>تبدیل تاریخ میلادی به شمسی بدون ساعت</summary>
	/// <result>1400/06/04</result>
	public static string ToPersianDate(this DateTime? value)
	{
		if (value is null)
			return string.Empty;
		
		PersianCalendar pc = new();
		var date = value.GetValueOrDefault();

		return $"{pc.GetYear(date):0000}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}";
	}

	/// <summary>تبدیل تاریخ میلادی به شمسی بدون ساعت</summary>
	/// <result>1400/06/04</result>
	public static string ToPersianDate(this DateTime value)
	{
		PersianCalendar pc = new();

		return $"{pc.GetYear(value):0000}/{pc.GetMonth(value):00}/{pc.GetDayOfMonth(value):00}";
	}

	public static string ToNormalDate(this string? value)
	{
		if (!value.HasValue())
			return "";

		var dateParts = value.Split("/");
		
		return $"{dateParts[0]}/{dateParts[1]:00}/{dateParts[2]:00}";

	}

	public static string ToMiladiDate(this string value)
	{
		var date = value.Split("/"); 

		PersianCalendar pc = new();
		DateTime dt = new(date[0].ToInt(), date[1].ToInt(), date[2].ToInt(), pc);
		return dt.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
	}


	public static string ToNormalTime(this string? value)
	{
		if (string.IsNullOrWhiteSpace(value)) return "";

		var timeParts = value.Split(":");

		return $"{timeParts[0]:00}:{timeParts[1]:00}";
	}

	public static string GetPersianDayOfWeekName(this DateTime date) => date.DayOfWeek switch
	{
		DayOfWeek.Saturday => "شنبه",
		DayOfWeek.Sunday => "يکشنبه",
		DayOfWeek.Monday => "دوشنبه",
		DayOfWeek.Tuesday => "سه‌شنبه",
		DayOfWeek.Wednesday => "چهارشنبه",
		DayOfWeek.Thursday => "پنجشنبه",
		DayOfWeek.Friday => "جمعه",
		_ => "",
	};

	public static string GetPersianMonthName(this DateTime date)
	{
		PersianCalendar pc = new();

		int month = pc.GetMonth(date);

		return month switch
		{
			1 => "فررودين",
			2 => "ارديبهشت",
			3 => "خرداد",
			4 => "تير",
			5 => "مرداد",
			6 => "شهريور",
			7 => "مهر",
			8 => "آبان",
			9 => "آذر",
			10 => "دي",
			11 => "بهمن",
			12 => "اسفند",
			_ => "",
		};
	}
}
