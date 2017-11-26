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

        private List<Hero> heroes;

        private List<byte[]> resources;

        private List<byte[]> mithrils;

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

            this.heroes = new List<Hero>();
            this.baseAddress = this.FindBaseAddress();
            if (this.baseAddress == IntPtr.Zero)
            {
                return false;
            }

            this.PopulateHero(this.baseAddress);
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

        internal void AddCreature(int heroIndex, string creatureNameToAdd, int amountToAdd)
        {
            var hero = this.heroes.SingleOrDefault(h => h.Index == heroIndex);
            var creatureTypeAddress = hero.Address + 0x6E;
            var creatureTypes = this.ReadMemory(creatureTypeAddress, Constants.CreatureAmount * 4);
            for (var i = 0; i < Constants.CreatureAmount * 4; i++)
            {
                var creatureType = BitConverter.ToUInt32(creatureTypes, 4 * i);
                if (creatureType == Constants.NullCreatureType)
                {
                    var newCreatureType = BitConverter.GetBytes(Constants.CreatureNames.IndexOf(creatureNameToAdd));
                    var newCreatureAmout = BitConverter.GetBytes(amountToAdd);
                    this.WriteMemory(creatureTypeAddress + 4 * i, newCreatureType);
                    this.WriteMemory(creatureTypeAddress + Constants.CreatureAmount * 4 + 4 * i, newCreatureAmout);
                    return;
                }
            }
        }

        internal void SetAllResources(int playerColorIndex, int resourceAmount, int mithrilAmount)
        {
            var resourceAmountBytes = BitConverter.GetBytes(resourceAmount);
            var resourceBaseAddress = this.GetResourceAddress(playerColorIndex);
            var resources = new byte[4 * Constants.BasicResourceTypeAmount];
            for (var i = 0; i < Constants.BasicResourceTypeAmount; i++)
            {
                for(var j = 0; j < 4; j++)
                {
                    resources[4 * i + j] = resourceAmountBytes[j];
                }
            }
            this.WriteMemory(resourceBaseAddress, resources);
            this.WriteMemory(Constants.MithrilAddress + (4 * playerColorIndex), BitConverter.GetBytes(mithrilAmount));
        }

        internal List<Hero> GetHeroes(int playerColor)
        {
            return this.heroes.Where(h => h.Color == playerColor).ToList();
        }

        internal void Save()
        {

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

        private void PopulateHero(IntPtr heroBaseAddress)
        {
            var currentHeroAddress = heroBaseAddress;
            for (var i = 0; i < Constants.HeroTotalAmount; i++)
            {
                if (this.HeroExist(currentHeroAddress))
                {
                    this.heroes.Add(new Hero
                    {
                        Address = currentHeroAddress,
                        BasicSkills = this.ReadMemory(currentHeroAddress + 0xA6, Constants.HeroBasicSkillAmount),
                        Color = this.ReadMemory(currentHeroAddress - 1, 1)[0],
                        Name = this.ReadMemory(currentHeroAddress, 12),
                        Index = i
                    });
                }
                currentHeroAddress += Constants.HeroMemorySize;
            }
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

        private IntPtr GetResourceAddress(int playerColorIndex)
        {
            return this.baseAddress
                - Constants.PlayerMemoryOffset
                + Constants.PlayerMemorySize * playerColorIndex;
        }
    }
}
