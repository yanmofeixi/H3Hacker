using H3Hacker.GameSettings;
using ProcessMemoryScanner;
using System;
using System.Collections.Generic;

namespace H3Hacker.Model
{
    internal class Hero : MemoryObject
    {
        private const int BasicSkillOffset = 0xA6;

        private const int CreatureOffset = 0x6E;

        private const int ItemOffset = 0x1B1;

        private const int ItemCountOffset = 0x3B1;

        private const int StatsOffset = 0x453;

        private const int ManaOffset = -0xB;

        private const int MovementPointOffset = 0x2A;

        internal const int MemorySize = 0x00000492;

        internal static int BasicSkillAmount = Constants.BasicSkillNames.Count;

        internal static int StatsAmount = Constants.StatsNames.Count;

        internal const int MaximumCreatureType = 7;

        internal const int ItemAmount = 64;

        internal Hero(IntPtr baseAddress, int playerIndex, int heroIndex) : base(baseAddress)
        {
            this.PlayerIndex = playerIndex;
            this.HeroIndex = heroIndex;
        }

        internal byte[] BasicSkills;

        internal Commander Commander;

        internal List<Creature> Creatures = new List<Creature>();

        internal List<Item> Items = new List<Item>();

        internal int ItemCount = 0;

        internal int HeroIndex;

        internal byte[] Name;

        internal int PlayerIndex;

        internal byte[] Stats;

        internal short Mana;

        internal int MovementPoint;

        internal override void Load(MemoryScanner memory)
        {
            this.BasicSkills = memory.ReadMemory(this.BaseAddress + BasicSkillOffset, (uint)BasicSkillAmount);
            this.Name = memory.ReadMemory(this.BaseAddress, 12);
            this.Stats = memory.ReadMemory(this.BaseAddress + StatsOffset, (uint)StatsAmount);
            this.Mana = memory.ReadMemory<short>(this.BaseAddress + ManaOffset);
            this.MovementPoint = memory.ReadMemory<int>(this.BaseAddress + MovementPointOffset);
            this.ItemCount = memory.ReadMemory<int>(this.BaseAddress + ItemCountOffset);
            for (var i = 0; i < MaximumCreatureType; i++)
            {
                var creature = new Creature(this.BaseAddress + CreatureOffset + 4 * i);
                creature.Load(memory);
                this.Creatures.Add(creature);
            }
            for (var i = 0; i < ItemAmount; i++)
            {
                var item = new Item(this.BaseAddress + ItemOffset + 8 * i);
                item.Load(memory);
                this.Items.Add(item);
            }
            this.Commander = new Commander(Constants.CommanderBaseAddress + this.HeroIndex * Commander.MemorySize);
            this.Commander.Load(memory);
        }

        internal override void Save(MemoryScanner memory)
        {
            memory.WriteMemory(this.BaseAddress + BasicSkillOffset, this.BasicSkills);
            memory.WriteMemory(this.BaseAddress + StatsOffset, this.Stats);
            memory.WriteMemory(this.BaseAddress + ManaOffset, this.Mana);
            memory.WriteMemory(this.BaseAddress + MovementPointOffset, this.MovementPoint);
            for (var i = 0; i < MaximumCreatureType; i++)
            {
                this.Creatures[i].Save(memory);
            }
            for (var i = 0; i < ItemAmount; i++)
            {
                this.Items[i].Save(memory);
            }
            this.Commander.Save(memory);
        }
    }
}
