using System;
using H3Hacker.GameSettings;

namespace H3Hacker.Model
{
    internal class CommanderItem : MemoryObject
    {
        private const short itemOffset = 146;

        public CommanderItem(IntPtr baseAddress) : base(baseAddress)
        {
        }

        internal byte[] Type;

        internal byte[] BattleTimes;

        internal override void Load(Func<IntPtr, uint, byte[]> readMemory)
        {
            this.Type = readMemory(this.BaseAddress, 2);
            this.BattleTimes = readMemory(this.BaseAddress + 2, 2);
        }

        internal override void Save(Action<IntPtr, byte[]> writeMemory)
        {
            writeMemory(this.BaseAddress, this.Type);
            writeMemory(this.BaseAddress + 2, this.BattleTimes);
        }

        internal bool Exist()
        {
            var index = BitConverter.ToInt16(this.Type, 0) - itemOffset;
            return index >= 0 && index < Constants.CommanderItems.Count;
        }

        internal static byte[] ToCommandItemType(int index)
        {
            var type = itemOffset + (short)index;
            return BitConverter.GetBytes(type);
        }
    }
}
