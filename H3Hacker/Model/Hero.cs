using System;
using System.Collections.Generic;
using H3Hacker.GameSettings;

namespace H3Hacker.Model
{
    internal class Hero : MemoryObject
    {
        private const int BasicSkillOffset = 0xA6;

        private const int CreatureOffset = 0x6E;

        private const int StatsOffset = 0x453;

        internal const int MemorySize = 0x00000492;

        internal const int BasicSkillAmount = 28;

        internal const int CreatureAmount = 7;

        internal const int StatsAmount = 4;

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

        internal byte[] Stats;

        internal override void Load(Func<IntPtr, uint, byte[]> readMemory)
        {
            this.BasicSkills = readMemory(this.BaseAddress + BasicSkillOffset, BasicSkillAmount);
            this.Name = readMemory(this.BaseAddress, 12);
            this.Stats = readMemory(this.BaseAddress + StatsOffset, StatsAmount);
            for (var i = 0; i < CreatureAmount; i++)
            {
                var creature = new Creature(this.BaseAddress + CreatureOffset + 4 * i);
                creature.Load(readMemory);
                this.Creatures.Add(creature);
            }
            this.Commander = new Commander(Constants.CommanderBaseAddress + this.HeroIndex * Commander.MemorySize);
            this.Commander.Load(readMemory);
        }

        internal override void Save(Action<IntPtr, byte[]> writeMemory)
        {
            writeMemory(this.BaseAddress + BasicSkillOffset, this.BasicSkills);
            writeMemory(this.BaseAddress + StatsOffset, this.Stats);
            for (var i = 0; i < CreatureAmount; i++)
            {
                this.Creatures[i].Save(writeMemory);
            }
        }

        internal int GetStat(int index)
        {
            return this.Stats[index];
        }

        internal void SetStat(int index, int value)
        {
            if (value >= 127)
            {
                value = 127;
            }
            else if (value < 0)
            {
                value = 0;
            }
            this.Stats[index] = (byte)value;
        }
    }
}
