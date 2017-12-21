using System;
using H3Hacker.GameSettings;
using ProcessMemoryScanner;

namespace H3Hacker.Model
{
    internal class CommanderItem : MemoryObject
    {
        private const short itemOffset = 0x92;

        public CommanderItem(IntPtr baseAddress) : base(baseAddress)
        {
        }

        internal short Type;

        internal short BattleTimes;

        internal override void Load(MemoryScanner memory)
        {
            this.Type = memory.ReadMemory<short>(this.BaseAddress);
            this.BattleTimes = memory.ReadMemory<short>(IntPtr.Add(this.BaseAddress, 2));
        }

        internal override void Save(MemoryScanner memory)
        {
            memory.WriteMemory(this.BaseAddress, this.Type);
            memory.WriteMemory(IntPtr.Add(this.BaseAddress, 2), this.BattleTimes);
        }

        internal bool Exist()
        {
            var index = this.Type - itemOffset;
            return index >= 0 && index < Constants.CommanderItems.Count;
        }

        internal static short ToCommandItemType(int index)
        {
            return (short)(itemOffset + (short)index);
        }
    }
}
