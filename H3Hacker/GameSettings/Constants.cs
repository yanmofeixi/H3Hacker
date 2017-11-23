using System;
using System.Collections.Generic;
using System.Text;

namespace H3Hacker.GameSettings
{
    public class Constants
    {
        public const int ComputerNameMemoryOffset = 0x000000CF;

        public const int MemoryScanStartAddress = 0x0800168B;

        public const int MemoryScanEndAddress = 0x0B00168B;

        public const int MemoryScanSkip = 0x00010000;

        public const int HeroTotalAmount = 156;

        public const int HeroMemorySize = 0x00000492;

        public const int PlayerMemoryOffset = 0x00000AD7;

        public const int PlayerMemorySize = 0x00000168;

        public const int MaxResourceAmount = 99999999;

        public static IntPtr MithrilAddress = new IntPtr(0x027F9A00);

        public static IntPtr CommanderBaseAddress = new IntPtr(0x028621B4);

        public const int CommanderMemorySize = 0x00000128;

        public static List<string> PlayerTypeNames = new List<string>
        {
            "电脑",
            "Player"
        };

        public static List<string> Colors = new List<string>
        {
            "红",
            "蓝",
            "褐",
            "绿",
            "橙",
            "紫",
            "青",
            "粉"
        };

        public static List<string> CommanderItems = new List<string>
        {
            "击碎之斧",
            "秘银之甲",
            "锋利之剑",
            "不朽之冠",
            "巫术项链",
            "加速之靴",
            "搜寻之弓",
            "龙眼戒",
            "硬化之盾",
            "斯拉瓦的力量之戒"
        };
    }
}
