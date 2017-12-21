using System;
using System.Collections.Generic;
using System.Linq;
using ProcessMemoryScanner;

namespace H3Hacker.Model
{
    internal class Commander : MemoryObject
    {
        internal const int BasicSkillAmount = 6;

        internal const int ItemAmount = 6;

        internal const int MemorySize = 0x00000128;

        internal Commander(IntPtr baseAddress) : base(baseAddress)
        {
        }

        internal int[] BasicSkills = new int[BasicSkillAmount];

        internal List<CommanderItem> Items = new List<CommanderItem>();
        
        internal void AddItem(int itemIndex, short battleTimes)
        {
            //already have this item
            if(this.Items.Any(i => i.Type == CommanderItem.ToCommandItemType(itemIndex)))
            {
                return;
            }
            var item = this.Items.FirstOrDefault(i => !i.Exist());
            if(item != null)
            {
                item.Type = CommanderItem.ToCommandItemType(itemIndex);
                item.BattleTimes = battleTimes;
            }
        }

        internal override void Load(MemoryScanner memory)
        {
            for (var i = 0; i < BasicSkillAmount; i++)
            {
                this.BasicSkills[i] = memory.ReadMemory<int>(IntPtr.Add(this.BaseAddress, -0xBC + 4 * i));
            }
            for (var i = 0; i < ItemAmount; i++)
            {
                var itemToAdd = new CommanderItem(IntPtr.Add(this.BaseAddress, - 0xA0 + 4 * 4 * i));
                itemToAdd.Load(memory);
                this.Items.Add(itemToAdd);
            }
        }

        internal override void Save(MemoryScanner memory)
        {
            //bug in wog, skill 4 appeared twice
            var additionalSkill = this.BasicSkills[4];
            for (var i = 0; i < BasicSkillAmount; i++)
            {
                memory.WriteMemory(IntPtr.Add(this.BaseAddress, -0xBC + 4 * i), this.BasicSkills[i]);
            }
            memory.WriteMemory(IntPtr.Add(this.BaseAddress, - 0xBC + 4 * BasicSkillAmount), additionalSkill);
            for (var i = 0; i < ItemAmount; i++)
            {
                this.Items[i].Save(memory);
            }
        }
    }
}
