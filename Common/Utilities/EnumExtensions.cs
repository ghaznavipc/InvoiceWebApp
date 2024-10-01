using Microsoft.AspNetCore.Mvc.Rendering;

namespace Common.Utilities;

public static class EnumExtensions
{
    public static SelectList GetEnumSelectList<T>(this T input) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException();
        var EntityState = new SelectList(Enum.GetValues(typeof(T)).Cast<T>().Select(v => new SelectListItem
        {
            Text = v.ToString(),
            Value = Convert.ToInt32(v).ToString()
        }).ToList(), "Value", "Text");
        return new SelectList(EntityState, "Value", "Text");
    }


    public static string GetDisplayName<T>(this T enumValue)
        where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum)
            return null;

        var Display = new DisplayAttribute();
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

        if (fieldInfo != null)
        {
            var attrs = fieldInfo.GetCustomAttributes();
            if (attrs != null && attrs.Any())
            {
                Display = (DisplayAttribute)attrs.FirstOrDefault();
            }
        }

        return Display.Name;
    }


    public static IEnumerable<T> GetEnumValues<T>(this T input) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException();

        return Enum.GetValues(input.GetType()).Cast<T>();
    }

    public static IEnumerable<T> GetEnumFlags<T>(this T input) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException();

        foreach (var value in Enum.GetValues(input.GetType()))
            if ((input as Enum).HasFlag(value as Enum))
                yield return (T)value;
    }

    public static string ToDisplay(this Enum value, DisplayProperty property = DisplayProperty.Name)
    {
        Assert.NotNull(value, nameof(value));

        var attribute = value.GetType().GetField(value.ToString())
            .GetCustomAttributes<DisplayAttribute>(false).FirstOrDefault();

        if (attribute == null)
            return value.ToString();

        var propValue = attribute.GetType().GetProperty(property.ToString()).GetValue(attribute, null);
        return propValue.ToString();
    }

    public static Dictionary<int, string> ToDictionary(this Enum value)
    {
        return Enum.GetValues(value.GetType()).Cast<Enum>().ToDictionary(Convert.ToInt32, q => ToDisplay(q));
    }
}

public enum DisplayProperty
{
    Description,
    GroupName,
    Name,
    Prompt,
    ShortName,
    Order
}
