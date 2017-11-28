using H3Hacker.Utility;
using System.Collections.Generic;
using System;

namespace H3Hacker.Model
{
    internal class Player : MemoryObject
    {
        internal const int BasicResourceTypeAmount = 7;

        internal const int MemorySize = 0x00000168;

        internal Player(IntPtr baseAddress, IntPtr mithrilAddress) : base(baseAddress)
        {
            this.MithrilAddress = mithrilAddress;
        }

        private IntPtr MithrilAddress;

        internal List<Hero> Heroes = new List<Hero>();

        internal byte[] BasicResources = new byte[4 * BasicResourceTypeAmount];

        internal byte[] Mithril = new byte[4];

        internal byte[] GetBasicResource(int resourceIndex)
        {
            return this.BasicResources.SubBytes(4 * resourceIndex, 4);
        }

        internal void SetBasicResource(int resourceIndex, int amount)
        {
            amount.CopyToByteArray(this.BasicResources, 4 * resourceIndex);
        }

        internal override void Load(Func<IntPtr, uint, byte[]> readMemory)
        {
            this.BasicResources = readMemory(this.BaseAddress, BasicResourceTypeAmount * 4);
            this.Mithril = readMemory(this.MithrilAddress, 4);
        }

        internal override void Save(Action<IntPtr, byte[]> writeMemory)
        {
            writeMemory(this.BaseAddress, this.BasicResources);
            writeMemory(this.MithrilAddress, this.Mithril);
            for (var i = 0; i < this.Heroes.Count; i++)
            {
                this.Heroes[i].Save(writeMemory);
            }
        }
    }
}
