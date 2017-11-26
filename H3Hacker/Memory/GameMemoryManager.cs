using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using H3Hacker.Model;
using H3Hacker.GameSettings;
using H3Hacker.Utility;

namespace H3Hacker.Memory
{
    internal class GameMemoryManager
    {
        private ProcessMemoryReaderApi.SYSTEM_INFO systemInformation;

        private Process gameProcess;

        private IntPtr handle;

        private List<Player> players;

        private IntPtr baseAddress;

        internal GameMemoryManager()
        {
            ProcessMemoryReaderApi.GetSystemInfo(out this.systemInformation);
        }

        internal bool OpenProcess()
        {
            this.gameProcess = Process.GetProcesses().FirstOrDefault(p => p.ProcessName == "h3era");

            if (this.gameProcess == null)
            {
                return false;
            }

            var access = ProcessMemoryReaderApi.ProcessAccessType.PROCESS_QUERY_INFORMATION |
                ProcessMemoryReaderApi.ProcessAccessType.PROCESS_VM_READ |
                ProcessMemoryReaderApi.ProcessAccessType.PROCESS_VM_WRITE |
                ProcessMemoryReaderApi.ProcessAccessType.PROCESS_VM_OPERATION;
            this.handle = ProcessMemoryReaderApi.OpenProcess((uint)access, 1, (uint)this.gameProcess.Id);
            this.baseAddress = this.FindBaseAddress();
            if (this.baseAddress == IntPtr.Zero)
            {
                return false;
            }

            this.LoadGame(this.baseAddress);
            return true;
        }

        internal void ModifyCommander(int heroIndex, List<string> itemsToAdd, int basicSkillLevel)
        {
            var commanderAddress = Constants.CommanderBaseAddress + heroIndex * Constants.CommanderMemorySize;
            var level = BitConverter.GetBytes(basicSkillLevel);
            var skills = new byte[4 * (Constants.CommanderBasicSkillAmount + 1)];
            for (var i = 0; i < Constants.CommanderBasicSkillAmount + 1; i++)
            {
                for(var j = 0; j < 4; j++)
                {
                    skills[4 * i + j] = level[j];
                }
            }
            this.WriteMemory(commanderAddress - 0xBC, skills);
            for (var i = 0; i < Constants.CommanderItemAmount; i++)
            {
                var itemIndex = (short)Constants.CommanderItems.IndexOf(itemsToAdd[i]);
                var itemBytes = BitConverter.GetBytes((short)(itemIndex + 0x92));
                this.WriteMemory(commanderAddress - 0xA0 + 0x10 * i, itemBytes);
            }
        }

        internal void AddCreature(int heroIndex, int playerIndex, string creatureNameToAdd, int amountToAdd)
        {
            var hero = this.players[playerIndex].Heroes.SingleOrDefault(h => h.HeroIndex == heroIndex);
            for (var i = 0; i < Constants.CreatureAmount; i++)
            {
                if (!this.CreatureExist(hero.Creatures[i].Type))
                {
                    hero.Creatures[i].Type = BitConverter.GetBytes(Constants.CreatureNames.IndexOf(creatureNameToAdd));
                    hero.Creatures[i].Amount = BitConverter.GetBytes(amountToAdd);
                    break;
                }
            }
            this.SaveGame();
        }

        internal void SetAllResources(int playerIndex, int basicResourceAmount, int mithrilAmount)
        {
            for(var i = 0; i < Constants.BasicResourceTypeAmount; i++)
            {
                this.players[playerIndex].SetBasicResource(i, BitConverter.GetBytes(basicResourceAmount));
            }
            this.players[playerIndex].Mithril = BitConverter.GetBytes(mithrilAmount);
            this.SaveGame();
        }

        internal List<Hero> GetHeroes(int playerIndex)
        {
            return this.players[playerIndex].Heroes;
        }

        internal void SaveGame()
        {
            for (var i = 0; i < Constants.PlayerAmount; i++)
            {
                var player = this.players[i];

                this.WriteMemory(
                    this.GetResourceAddress(i),
                    player.BasicResources) ;
                this.WriteMemory(
                    Constants.MithrilAddress + 4 * i,
                    player.Mithril);

                for (var j = 0; j < player.Heroes.Count; j++)
                {
                    var hero = player.Heroes[j];

                    this.WriteMemory(
                        hero.Address + 0x6E,
                        hero.GetCreatures());
                }
            }
        }

        private byte[] ReadMemory(IntPtr address, uint byteArrayLength)
        {
            var lpNumberOfBytesRead = IntPtr.Zero;
            var buffer = new byte[byteArrayLength];
            ProcessMemoryReaderApi.ReadProcessMemory(this.handle, address, buffer, byteArrayLength, out lpNumberOfBytesRead);
            return buffer;
        }

        private void WriteMemory(IntPtr address, byte[] data)
        {
            var length = data.Length;
            var lpNumberOfBytesWritten = IntPtr.Zero;
            ProcessMemoryReaderApi.WriteProcessMemory(this.handle, address, data, (uint)data.Length, out lpNumberOfBytesWritten);
        }

        private IntPtr FindBaseAddress()
        {
            for (var address = Constants.MemoryScanStartAddress;
                        address < Constants.MemoryScanEndAddress;
                        address += Constants.MemoryScanSkip)
            {
                if (IsAddressName(new IntPtr(address)))
                {
                    return new IntPtr(address);
                }
            }
            return IntPtr.Zero;
        }

        private void LoadGame(IntPtr heroBaseAddress)
        {
            this.players = new List<Player>();
            for (var i = 0; i < Constants.PlayerAmount; i++)
            {
                this.players.Add(new Player());
                this.players[i].BasicResources = this.ReadMemory(
                    this.GetResourceAddress(i), 
                    Constants.BasicResourceTypeAmount * 4);
            }

            var currentHeroAddress = heroBaseAddress;
            for (var i = 0; i < Constants.HeroTotalAmount; i++)
            {
                if (this.HeroExist(currentHeroAddress))
                {
                    var heroToAdd = new Hero
                    {
                        Address = currentHeroAddress,
                        BasicSkills = this.ReadMemory(currentHeroAddress + 0xA6, Constants.HeroBasicSkillAmount),
                        PlayerIndex = this.ReadMemory(currentHeroAddress - 1, 1)[0],
                        Name = this.ReadMemory(currentHeroAddress, 12),
                        HeroIndex = i
                    };

                    var creatureData = this.ReadMemory(
                        currentHeroAddress + 0x6E,
                        4 * 2 * Constants.CreatureAmount);

                    for (var j = 0; j < Constants.CreatureAmount; j++)
                    {
                        heroToAdd.Creatures.Add(new Creature
                        {
                            Type = creatureData.GetSubBytes(4 * j, 4),
                            Amount = creatureData.GetSubBytes(4 * j + 4 * Constants.CreatureAmount, 4)
                        });
                    }
                    this.players[heroToAdd.PlayerIndex].Heroes.Add(heroToAdd);
                }
                currentHeroAddress += Constants.HeroMemorySize;
            }
        }

        private bool CreatureExist(byte[] type)
        {
            return BitConverter.ToUInt32(type, 0) != Constants.NullCreatureType;
        }

        private bool HeroExist(IntPtr address)
        {
            return this.ReadMemory(address - 1, 1)[0] != 0xFF;
        }

        private bool IsAddressName(IntPtr address)
        {
            var memory = this.ReadMemory(address - Constants.ComputerNameMemoryOffset, 9);
            if (Constants.PlayerTypeNames.Any(name => name == memory.ToStringGBK()))
            {
                return true;
            }
            return false;
        }

        private IntPtr GetResourceAddress(int playerIndex)
        {
            return this.baseAddress
                - Constants.PlayerMemoryOffset
                + Constants.PlayerMemorySize * playerIndex;
        }
    }
}
