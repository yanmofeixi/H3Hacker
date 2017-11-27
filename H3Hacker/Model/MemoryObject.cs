using System;

namespace H3Hacker.Model
{
    internal abstract class MemoryObject
    {
        internal MemoryObject(IntPtr baseAddress)
        {
            this.BaseAddress = baseAddress;
        }

        protected IntPtr BaseAddress;

        internal abstract void Load(Func<IntPtr, uint, byte[]> readMemory);

        internal abstract void Save(Action<IntPtr, byte[]> writeMemory);
    }
}
