﻿using System;
using System.Collections.Generic;

namespace H3Hacker.GameSettings
{
    public class Constants
    {
        public const int ComputerNameMemoryOffset = 0x000000CF;

        public const int MemoryScanStartAddress = 0x0800168B;

        public const int MemoryScanEndAddress = 0x0B00168B;

        public const int MemoryScanSkip = 0x00010000;

        public const int HeroTotalAmount = 156;

        public const int HeroBasicSkillAmount = 28;

        public const int HeroMemorySize = 0x00000492;

        public const int PlayerMemoryOffset = 0x00000AD7;

        public const int PlayerMemorySize = 0x00000168;

        public const uint NullCreatureType = 0xFFFFFFFF;

        public const int BasicResourceTypeAmount = 7;

        public const int CreatureAmount = 7;

        public static IntPtr MithrilAddress = new IntPtr(0x027F9A00);

        public static IntPtr CommanderBaseAddress = new IntPtr(0x028621B4);

        public const int CommanderMemorySize = 0x00000128;

        public const int CommanderBasicSkillAmount = 6;

        public const int CommanderItemAmount = 6;

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

        public static List<string> CreatureNames = new List<string>
        {
            "枪兵",
            "戟兵",
            "弓箭手",
            "神射手",
            "狮鹫",
            "皇家狮鹫",
            "剑士",
            "十字军",
            "僧侣",
            "祭司",
            "骑兵",
            "骑士",
            "天使",
            "大天使",
            "半人马",
            "半人马队长",
            "矮人",
            "战斗矮人",
            "木精灵",
            "大地精灵",
            "飞马",
            "银色飞马",
            "枯木卫士",
            "枯木战士",
            "独角兽",
            "战争独角兽",
            "绿龙",
            "黄金龙",
            "小妖精",
            "大妖精",
            "石像鬼",
            "高级石像鬼",
            "石头人",
            "铁皮人",
            "法师",
            "大法师",
            "神怪",
            "神怪主",
            "那加",
            "那加女王",
            "巨人",
            "泰坦巨人",
            "小恶魔",
            "恶魔之子",
            "哥革",
            "玛革",
            "地狱犬",
            "冥府三头犬",
            "恶鬼",
            "长角恶鬼",
            "地穴魔",
            "地穴领主",
            "火怪",
            "火怪苏丹",
            "恶魔",
            "大恶魔",
            "骷髅兵",
            "骷髅战士",
            "丧尸",
            "僵尸",
            "幽灵",
            "阴魂",
            "吸血鬼",
            "吸血伯爵",
            "尸巫",
            "尸巫王",
            "暗黑骑士",
            "恐怖骑士",
            "骨龙",
            "幽灵龙",
            "洞穴人",
            "地狱洞穴人",
            "鸟身女妖",
            "鸟身女巫",
            "邪眼",
            "毒眼",
            "美杜莎",
            "美杜莎女王",
            "牛头怪",
            "牛头怪国王",
            "蝎狮",
            "毒蝎狮",
            "红龙",
            "黑龙",
            "大耳怪",
            "大耳怪王",
            "恶狼骑士",
            "恶狼斗士",
            "半兽人",
            "半兽人酋长",
            "食人魔",
            "食人魔酋长",
            "大鹏",
            "雷鸟",
            "独眼巨人",
            "独眼巨人王",
            "巨兽",
            "远古巨兽",
            "狼头怪",
            "狼头怪队长",
            "蜥蜴人",
            "蜥蜴勇士",
            "蛇皮兽",
            "强力蛇皮兽",
            "毒蝇",
            "龙蝇",
            "蛇蜥",
            "巨蜥",
            "飞龙",
            "飞龙王",
            "九头怪",
            "混乱九头怪",
            "气元素",
            "土元素",
            "火元素",
            "水元素",
            "金人",
            "钻石人",
            "小仙子",
            "仙子",
            "精神元素",
            "魔法元素",
            "N/A",
            "冰元素",
            "N/A",
            "岩浆元素",
            "N/A",
            "雷元素",
            "N/A",
            "能量元素",
            "火鸟",
            "凤凰",
            "圣龙",
            "水晶龙",
            "仙女龙",
            "锈龙",
            "幻影法师",
            "幻影射手",
            "侏儒",
            "农民",
            "野猪",
            "木乃伊",
            "游牧民",
            "盗贼",
            "桥梁怪",
            "N/A",
            "N/A",
            "N/A",
            "N/A",
            "N/A",
            "至高天使长",
            "钻石龙",
            "雷神",
            "地狱男爵",
            "血龙",
            "暗黑龙",
            "幽灵比蒙",
            "地狱九头怪",
            "神圣凤凰",
            "鬼魂",
            "战争之神",
            "和平之神",
            "魔法之神",
            "知识之神",
            "火元素使者",
            "土元素使者",
            "气元素使者",
            "水元素使者",
            "血污怪",
            "战争狂热者",
            "极地神射手",
            "熔岩神射手",
            "梦魇",
            "圣侏儒",
            "圣骑士",
            "圣师",
            "圣堂守卫",
            "女妖",
            "收魂使者",
            "嗜杀者",
            "食人魔领袖",
            "萨满",
            "星界灵",
            "圣骑士1",
            "圣师1",
            "圣堂守卫1",
            "女妖1",
            "收魂使者1",
            "嗜杀者1",
            "食人魔领袖1",
            "萨满1",
            "星界灵1",
            "席尔瓦半人马",
            "女巫",
            "变狼人",
            "地狱战马",
            "龙巫妖"
        };

        public static List<string> BasicSkillNames = new List<string>
        {
            "箭术",
            "寻路术",
            "后勤学",
            "侦察术",
            "外交术",
            "航海术",
            "领导术",
            "智慧术",
            "神秘术",
            "幸运术",
            "弹道术",
            "鹰眼术",
            "招魂术",
            "理财术",
            "火系魔法",
            "气系魔法",
            "水系魔法",
            "土系魔法",
            "学术",
            "战术",
            "炮术",
            "学习能力",
            "进攻术",
            "防御术",
            "智力",
            "魔力",
            "抵抗力",
            "急救术"
        };

        public static List<string> BasicSkillLevelNames = new List<string>
        {
            "无",
            "初级",
            "中级",
            "高级"
        };
    }
}
