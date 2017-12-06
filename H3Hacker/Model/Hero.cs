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

        private const int ManaOffset = -0xB;

        private const int MovementPointOffset = 0x2A;

        internal const int MemorySize = 0x00000492;

        internal static int BasicSkillAmount = Constants.BasicSkillNames.Count;

        internal static int StatsAmount = Constants.StatsNames.Count;

        internal const int CreatureAmount = 7;

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

        internal byte[] Mana;

        internal byte[] MovementPoint;

        internal override void Load(Func<IntPtr, uint, byte[]> readMemory)
        {
            this.BasicSkills = readMemory(IntPtr.Add(this.BaseAddress, BasicSkillOffset), (uint) BasicSkillAmount);
            this.Name = readMemory(this.BaseAddress, 12);
            this.Stats = readMemory(IntPtr.Add(this.BaseAddress, StatsOffset), (uint) StatsAmount);
            this.Mana = readMemory(IntPtr.Add(this.BaseAddress, ManaOffset), 2);
            this.MovementPoint = readMemory(IntPtr.Add(this.BaseAddress, MovementPointOffset), 4);
            for (var i = 0; i < CreatureAmount; i++)
            {
                var creature = new Creature(IntPtr.Add(this.BaseAddress, CreatureOffset + 4 * i));
                creature.Load(readMemory);
                this.Creatures.Add(creature);
            }
            this.Commander = new Commander(IntPtr.Add(Constants.CommanderBaseAddress, this.HeroIndex * Commander.MemorySize));
            this.Commander.Load(readMemory);
        }

        internal override void Save(Action<IntPtr, byte[]> writeMemory)
        {
            writeMemory(IntPtr.Add(this.BaseAddress, BasicSkillOffset), this.BasicSkills);
            writeMemory(IntPtr.Add(this.BaseAddress, StatsOffset), this.Stats);
            writeMemory(IntPtr.Add(this.BaseAddress, ManaOffset), this.Mana);
            writeMemory(IntPtr.Add(this.BaseAddress, MovementPointOffset), this.MovementPoint);
            for (var i = 0; i < CreatureAmount; i++)
            {
                this.Creatures[i].Save(writeMemory);
            }
            this.Commander.Save(writeMemory);
        }
    }
}
