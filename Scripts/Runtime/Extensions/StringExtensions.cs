using System;

public static partial class StringExtensions
{
    public static string FirstToUpper(this string value)
    {
        return value.Length switch
        {
            0 => value,
            1 => value.ToUpper(),
            _ => value.Substring(0, 1).ToUpper() + value.Substring(1)
        };
    }

    public static string FirstToLower(this string value)
    {
        return value.Length switch
        {
            0 => value,
            1 => value.ToLower(),
            _ => value.Substring(0, 1).ToLower() + value.Substring(1)
        };
    }

    public static string Replace(this string originalString, string oldValue, string newValue, StringComparison comparisonType)
    {
        int startIndex = 0;
        while (true)
        {
            startIndex = originalString.IndexOf(oldValue, startIndex, comparisonType);
            if (startIndex == -1)
                break;

            originalString = originalString.Substring(0, startIndex) + newValue + originalString.Substring(startIndex + oldValue.Length);

            startIndex += newValue.Length;
        }
        return originalString;
    }

    public static string Replace(this string value, char[] oldChars, string newValue)
    {
        string[] temp;

        temp = value.Split(oldChars);
        return String.Join(newValue, temp);
    }
}
