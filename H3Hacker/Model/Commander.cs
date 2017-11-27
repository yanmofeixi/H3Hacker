using System;
using H3Hacker.GameSettings;
using H3Hacker.Utility;
using System.Collections.Generic;
using System.Linq;

namespace H3Hacker.Model
{
    internal class Commander : MemoryObject
    {
        internal Commander(IntPtr baseAddress) : base(baseAddress)
        {
        }

        internal byte[] BasicSkills;

        internal List<CommanderItem> Items = new List<CommanderItem>();

        internal void SetSkill(int index, int level)
        {
            level.CopyToByteArray(this.BasicSkills, 4 * index);
        }

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
                item.BattleTimes = BitConverter.GetBytes(battleTimes);
            }
        }

        internal override void Load(Func<IntPtr, uint, byte[]> readMemory)
        {
            this.BasicSkills = readMemory(this.BaseAddress - 0xBC, 4 * Constants.CommanderBasicSkillAmount);
            for (var i = 0; i < Constants.CommanderItemAmount; i++)
            {
                var itemToAdd = new CommanderItem(this.BaseAddress - 0xA0 + 4 * 4 * i);
                itemToAdd.Load(readMemory);
                this.Items.Add(itemToAdd);
            }
        }

        internal override void Save(Action<IntPtr, byte[]> writeMemory)
        {
            //bug in wog, skill 4 appeared twice
            var additionalSkill = this.BasicSkills.SubBytes(16, 4);
            writeMemory(this.BaseAddress - 0xBC, this.BasicSkills);
            writeMemory(this.BaseAddress - 0xBC + 4 * Constants.CommanderBasicSkillAmount, additionalSkill);
            for (var i = 0; i < Constants.CommanderItemAmount; i++)
            {
                this.Items[i].Save(writeMemory);
            }
        }
    }
}
