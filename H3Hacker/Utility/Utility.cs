using System;
using System.Text;

namespace H3Hacker.Utility
{
    public static class Utility
    {
        public static string ToStringByEncoding(this byte[] data, Encoding encoding = null)
        {
            if (encoding == null)
            {
                return BitConverter.ToString(data).Replace("-", "");
            }
            var rawString = encoding.GetString(data);
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
