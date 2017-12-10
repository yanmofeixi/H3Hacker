using System;
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

        internal static void CopyToByteArray(this short shortToCopy, byte[] byteArray, int startIndex)
        {
            var bytes = BitConverter.GetBytes(shortToCopy);
            for (var i = 0; i < 2; i++)
            {
                byteArray[startIndex + i] = bytes[i];
            }
        }

        internal static void CopyToByteArray(this int intToCopy, byte[] byteArray, int startIndex)
        {
            var bytes = BitConverter.GetBytes(intToCopy);
            for (var i = 0; i < 4; i++)
            {
                byteArray[startIndex + i] = bytes[i];
            }
        }
    }
}
