using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using H3Hacker.Model;
using H3Hacker.GameSettings;
using ProcessMemoryScanner;

namespace H3Hacker.GameMemory
{
    internal class GameMemoryManager : IDisposable
    {
        private Game game;

        private MemoryScanner memory;

        internal bool OpenProcess()
        {
            try
            {
                this.memory = new MemoryScanner(p => p.ProcessName == "h3era");
                this.game = new Game(this.FindBaseAddress());
            }
            catch(InvalidOperationException)
            {
                return false;
            }
            if (!this.game.IsAddressValid)
            {
                return false;
            }
            this.game.Load(this.memory);
            return true;
        }

        internal void ModifyCommander(int heroIndex, int playerIndex, List<string> itemsToAdd, int basicSkillLevel)
        {
            var commander = this.game.Players[playerIndex].Heroes.SingleOrDefault(h => h.HeroIndex == heroIndex).Commander;
            for (var i = 0; i < Commander.BasicSkillAmount; i++)
            {
                commander.BasicSkills[i] = basicSkillLevel;
            }
            for (var i = 0; i < itemsToAdd.Count; i++)
            {
                var itemIndex = Constants.CommanderItems.IndexOf(itemsToAdd[i]);
                commander.AddItem(itemIndex, 0);
            }
            commander.Save(this.memory);
        }

        internal void AddCreature(int heroIndex, int playerIndex, string creatureNameToAdd, int amountToAdd)
        {
            var hero = this.game.Players[playerIndex].Heroes.SingleOrDefault(h => h.HeroIndex == heroIndex);
            for (var i = 0; i < Hero.CreatureAmount; i++)
            {
                if (!hero.Creatures[i].Exist())
                {
                    hero.Creatures[i].Type = Constants.CreatureNames.IndexOf(creatureNameToAdd);
                    hero.Creatures[i].Amount = amountToAdd;
                    hero.Creatures[i].Save(this.memory);
                    break;
                }
            }
        }

        internal void SetAllResources(int playerIndex, int basicResourceAmount, int mithrilAmount)
        {
            for(var i = 0; i < Player.BasicResourceTypeAmount; i++)
            {
                this.game.Players[playerIndex].BasicResources[i] = basicResourceAmount;
            }
            this.game.Players[playerIndex].Mithril = mithrilAmount;
            this.game.Players[playerIndex].Save(this.memory);
        }

        internal List<Hero> GetHeroes(int playerIndex)
        {
            return this.game.Players[playerIndex].Heroes;
        }

        internal void SaveGame()
        {
            this.game.Save(this.memory);
        }

        private IntPtr FindBaseAddress()
        {
            var memoryRegions = this.memory.FindMemoryRegion(m => 
            m.State == 0x01000 && 
            m.Protect == 0x4 &&
            m.RegionSize.ToInt32() == 0xFF000);

            const int MinNameBytesLength = 4;

            foreach(var memoryRegion in memoryRegions)
            {
                foreach(var name in Constants.PlayerTypeNames)
                {
                    var nameBytes = Encoding.GetEncoding(Constants.Encoding).GetBytes(name);
                    var address = this.memory.FindByAoB(nameBytes, memoryRegion);
                    if(address == IntPtr.Zero)
                    {
                        continue;
                    }
                    while (true)
                    {
                        var nameRead = Encoding.GetEncoding(Constants.Encoding).GetString(
                            this.memory.ReadMemory(address + Player.MemorySize, MinNameBytesLength));
                        if (Constants.PlayerTypeNames.All(n => !n.Contains(nameRead)))
                        {
                            return address + Player.NameOffset;
                        }
                        address += Player.MemorySize;
                        if(address.ToInt64() > memoryRegion.BaseAddress.ToInt64() + memoryRegion.RegionSize.ToInt64())
                        {
                            break;
                        }
                    }
                }
            }
            return IntPtr.Zero;
        }

        public void Dispose()
        {
            this.memory.Dispose();
        }
    }
}
