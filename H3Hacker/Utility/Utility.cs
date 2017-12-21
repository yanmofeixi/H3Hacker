using System.Text;
using H3Hacker.GameSettings;

namespace H3Hacker.Utility
{
    internal static class Utility
    {
        internal static string ToStringByEncoding(this byte[] data)
        {
            var rawString = Encoding.GetEncoding(Constants.Encoding).GetString(data);
            for (var i = 0; i < rawString.Length; i++)
            {
                if (rawString[i] == '\0')
                {
                    return rawString.Substring(0, i);
                }
            }
            return rawString;
        }
    }
}
