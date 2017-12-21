using System;
using ProcessMemoryScanner;

namespace H3Hacker.Model
{
    internal abstract class MemoryObject
    {
        internal MemoryObject(IntPtr baseAddress)
        {
            this.BaseAddress = baseAddress;
        }

        protected IntPtr BaseAddress;

        internal abstract void Load(MemoryScanner memory);

        internal abstract void Save(MemoryScanner memory);
    }
}
