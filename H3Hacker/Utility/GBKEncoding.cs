using System.Text;

namespace H3Hacker.Utility
{
    internal static class GBKEncoding
    {
        internal static string ToStringGBK(this byte[] data)
        {
            var rawString = Encoding.GetEncoding("GBK").GetString(data);
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
