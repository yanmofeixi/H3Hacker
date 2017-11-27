using System;
using H3Hacker.GameSettings;

namespace H3Hacker.Model
{
    internal class Creature : MemoryObject
    {
        internal Creature(IntPtr baseAddress) : base(baseAddress)
        {
        }

        internal byte[] Amount = new byte[4];

        internal byte[] Type = new byte[4];

        internal override void Load(Func<IntPtr, uint, byte[]> readMemory)
        {
            this.Type = readMemory(this.BaseAddress, 4);
            this.Amount = readMemory(this.BaseAddress + 4 * Constants.CreatureAmount, 4);
        }

        internal override void Save(Action<IntPtr, byte[]> writeMemory)
        {
            writeMemory(this.BaseAddress, this.Type);
            writeMemory(this.BaseAddress + 4 * Constants.CreatureAmount, this.Amount);
        }

        internal bool Exist()
        {
            return BitConverter.ToUInt32(this.Type, 0) != Constants.NullCreatureType;
        }
    }
}
