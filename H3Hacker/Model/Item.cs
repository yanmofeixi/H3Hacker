using System;
using ProcessMemoryScanner;

namespace H3Hacker.Model
{
    internal class Item : MemoryObject
    {
        private const int NullItemType = -1;

        internal Item(IntPtr baseAddress) : base(baseAddress)
        {
        }

        internal int Type = NullItemType;

        internal bool Exist()
        {
            return this.Type != NullItemType;
        }

        internal override void Load(MemoryScanner memory)
        {
            this.Type = memory.ReadMemory<int>(this.BaseAddress);
        }

        internal override void Save(MemoryScanner memory)
        {
            memory.WriteMemory(this.BaseAddress, this.Type);
        }
    }
}
