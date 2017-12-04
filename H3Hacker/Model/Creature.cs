using System;

namespace H3Hacker.Model
{
    internal class Creature : MemoryObject
    {
        private const uint NullCreatureType = 0xFFFFFFFF;

        internal Creature(IntPtr baseAddress) : base(baseAddress)
        {
        }

        internal byte[] Amount = new byte[4];

        internal byte[] Type = new byte[4];

        internal override void Load(Func<IntPtr, uint, byte[]> readMemory)
        {
            this.Type = readMemory(this.BaseAddress, 4);
            this.Amount = readMemory(IntPtr.Add(this.BaseAddress, 4 * Hero.CreatureAmount), 4);
        }

        internal override void Save(Action<IntPtr, byte[]> writeMemory)
        {
            writeMemory(this.BaseAddress, this.Type);
            writeMemory(IntPtr.Add(this.BaseAddress, 4 * Hero.CreatureAmount), this.Amount);
        }

        internal bool Exist()
        {
            return BitConverter.ToUInt32(this.Type, 0) != NullCreatureType;
        }
    }
}
