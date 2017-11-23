using System.Text;

namespace H3Hacker.Utility
{
    internal static class Utility
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

        internal static byte[] GetSubBytes(this byte[] data, int startIndex, int Length)
        {
            var result = new byte[Length];
            for(var i = 0; i < Length; i++)
            {
                result[i] = data[i + startIndex];
            }
            return result;
        }
    }
}
