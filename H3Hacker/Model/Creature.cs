﻿using ProcessMemoryScanner;
using System;

namespace H3Hacker.Model
{
    internal class Creature : MemoryObject
    {
        private const int NullCreatureType = -1;

        internal Creature(IntPtr baseAddress) : base(baseAddress)
        {
        }

        internal int Amount = 0;

        internal int Type = NullCreatureType;

        internal override void Load(MemoryScanner memory)
        {
            this.Type = memory.ReadMemory<int>(this.BaseAddress);
            this.Amount = memory.ReadMemory<int>(this.BaseAddress + 4 * Hero.MaximumCreatureType);
        }

        internal override void Save(MemoryScanner memory)
        {
            memory.WriteMemory(this.BaseAddress, this.Type);
            memory.WriteMemory(this.BaseAddress + 4 * Hero.MaximumCreatureType, this.Amount);
        }

        internal bool Exist()
        {
            return this.Type != NullCreatureType;
        }

        internal void Remove()
        {
            this.Amount = 0;
            this.Type = NullCreatureType;
        }
    }
}
