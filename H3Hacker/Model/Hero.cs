using H3Hacker.GameSettings;
using H3Hacker.Utility;
using System;
using System.Collections.Generic;

namespace H3Hacker.Model
{
    internal class Hero : MemoryObject
    {
        internal Hero(IntPtr baseAddress, int playerIndex, int heroIndex) : base(baseAddress)
        {
            this.PlayerIndex = playerIndex;
            this.HeroIndex = heroIndex;
        }

        internal byte[] BasicSkills;

        internal Commander Commander;

        internal List<Creature> Creatures = new List<Creature>();

        internal int HeroIndex;

        internal byte[] Name;

        internal int PlayerIndex;

        internal override void Load(Func<IntPtr, uint, byte[]> readMemory)
        {
            this.BasicSkills = readMemory(this.BaseAddress + 0xA6, Constants.HeroBasicSkillAmount);
            this.Name = readMemory(this.BaseAddress, 12);
            for (var i = 0; i < Constants.CreatureAmount; i++)
            {
                var creature = new Creature(this.BaseAddress + 0x6E + 4 * i);
                creature.Load(readMemory);
                this.Creatures.Add(creature);
            }
            this.Commander = new Commander(Constants.CommanderBaseAddress + this.HeroIndex * Constants.CommanderMemorySize);
            this.Commander.Load(readMemory);
        }

        internal override void Save(Action<IntPtr, byte[]> writeMemory)
        {
            writeMemory(this.BaseAddress + 0xA6, this.BasicSkills);
            for (var i = 0; i < Constants.CreatureAmount; i++)
            {
                this.Creatures[i].Save(writeMemory);
            }
        }
    }
}
