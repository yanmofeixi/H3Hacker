﻿using System;
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
