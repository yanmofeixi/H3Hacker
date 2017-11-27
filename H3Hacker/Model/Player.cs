﻿using H3Hacker.GameSettings;
using H3Hacker.Utility;
using System.Collections.Generic;
using System;

namespace H3Hacker.Model
{
    internal class Player : MemoryObject
    {
        internal Player(IntPtr baseAddress, IntPtr mithrilAddress) : base(baseAddress)
        {
            this.MithrilAddress = mithrilAddress;
        }

        private IntPtr MithrilAddress;

        internal List<Hero> Heroes = new List<Hero>();

        internal byte[] BasicResources = new byte[4 * Constants.BasicResourceTypeAmount];

        internal byte[] Mithril = new byte[4];

        internal byte[] GetBasicResource(int resourceIndex)
        {
            return this.BasicResources.SubBytes(4 * resourceIndex, 4);
        }

        internal void SetBasicResource(int resourceIndex, byte[] amount)
        {
            for (var i = 0; i < 4; i++)
            {
                this.BasicResources[4 * resourceIndex + i] = amount[i];
            }
        }

        internal override void Load(Func<IntPtr, uint, byte[]> readMemory)
        {
            this.BasicResources = readMemory(this.BaseAddress, Constants.BasicResourceTypeAmount * 4);
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