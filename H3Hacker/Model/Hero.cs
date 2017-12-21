using System;
using System.Collections.Generic;
using H3Hacker.GameSettings;
using ProcessMemoryScanner;

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

        internal short Mana;

        internal int MovementPoint;

        internal override void Load(MemoryScanner memory)
        {
            this.BasicSkills = memory.ReadMemory(IntPtr.Add(this.BaseAddress, BasicSkillOffset), (uint) BasicSkillAmount);
            this.Name = memory.ReadMemory(this.BaseAddress, 12);
            this.Stats = memory.ReadMemory(IntPtr.Add(this.BaseAddress, StatsOffset), (uint) StatsAmount);
            this.Mana = memory.ReadMemory<short>(IntPtr.Add(this.BaseAddress, ManaOffset));
            this.MovementPoint = memory.ReadMemory<int>(IntPtr.Add(this.BaseAddress, MovementPointOffset));
            for (var i = 0; i < CreatureAmount; i++)
            {
                var creature = new Creature(IntPtr.Add(this.BaseAddress, CreatureOffset + 4 * i));
                creature.Load(memory);
                this.Creatures.Add(creature);
            }
            this.Commander = new Commander(IntPtr.Add(Constants.CommanderBaseAddress, this.HeroIndex * Commander.MemorySize));
            this.Commander.Load(memory);
        }

        internal override void Save(MemoryScanner memory)
        {
            memory.WriteMemory(IntPtr.Add(this.BaseAddress, BasicSkillOffset), this.BasicSkills);
            memory.WriteMemory(IntPtr.Add(this.BaseAddress, StatsOffset), this.Stats);
            memory.WriteMemory(IntPtr.Add(this.BaseAddress, ManaOffset), this.Mana);
            memory.WriteMemory(IntPtr.Add(this.BaseAddress, MovementPointOffset), this.MovementPoint);
            for (var i = 0; i < CreatureAmount; i++)
            {
                this.Creatures[i].Save(memory);
            }
            this.Commander.Save(memory);
        }
    }
}
