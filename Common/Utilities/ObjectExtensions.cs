namespace Common.Utilities;

public static class ObjectExtensions
{
    public static void CheckArgumentIsNull(this object o, string paramName)
    {
        if (o == null)
            throw new ArgumentNullException(paramName);
    }
}
