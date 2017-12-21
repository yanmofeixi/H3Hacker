using System.Collections.Generic;
using System;
using ProcessMemoryScanner;

namespace H3Hacker.Model
{
    internal class Player : MemoryObject
    {
        internal const int BasicResourceTypeAmount = 7;

        internal const int MemorySize = 0x00000168;

        internal const int NameOffset = 0x000000CF;

        internal Player(IntPtr baseAddress, IntPtr mithrilAddress) : base(baseAddress)
        {
            this.MithrilAddress = mithrilAddress;
        }

        private IntPtr MithrilAddress;

        internal List<Hero> Heroes = new List<Hero>();

        internal int[] BasicResources = new int[BasicResourceTypeAmount];

        internal int Mithril;

        internal override void Load(MemoryScanner memory)
        {
            for(var i = 0; i < BasicResourceTypeAmount; i++)
            {
                this.BasicResources[i] = memory.ReadMemory<int>(IntPtr.Add(this.BaseAddress, 4 * i));
            }
            this.Mithril = memory.ReadMemory<int>(this.MithrilAddress);
        }

        internal override void Save(MemoryScanner memory)
        {
            for (var i = 0; i < BasicResourceTypeAmount; i++)
            {
                memory.WriteMemory(IntPtr.Add(this.BaseAddress, 4 * i), this.BasicResources[i]);
            }
            memory.WriteMemory(this.MithrilAddress, this.Mithril);
            for (var i = 0; i < this.Heroes.Count; i++)
            {
                this.Heroes[i].Save(memory);
            }
        }
    }
}
