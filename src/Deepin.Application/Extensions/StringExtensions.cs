namespace Deepin.Application.Extensions;

public static class StringExtensions
{

    public static string ToNomalized(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        return char.ToUpper(str[0]) + str.Substring(1).ToLower();
    }
}
