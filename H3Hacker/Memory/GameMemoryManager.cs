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

        private Game game;

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
            this.game = new Game(this.FindBaseAddress());
            if (!this.game.IsAddressValid)
            {
                return false;
            }
            this.game.Load(this.ReadMemory);
            return true;
        }

        internal void ModifyCommander(int heroIndex, int playerIndex, List<string> itemsToAdd, int basicSkillLevel)
        {
            var commander = this.game.Players[playerIndex].Heroes.SingleOrDefault(h => h.HeroIndex == heroIndex).Commander;
            for (var i = 0; i < Constants.CommanderBasicSkillAmount; i++)
            {
                commander.SetSkill(i, basicSkillLevel);
            }
            for (var i = 0; i < itemsToAdd.Count; i++)
            {
                var itemIndex = Constants.CommanderItems.IndexOf(itemsToAdd[i]);
                commander.AddItem(itemIndex, 0);
            }
            commander.Save(this.WriteMemory);
        }

        internal void AddCreature(int heroIndex, int playerIndex, string creatureNameToAdd, int amountToAdd)
        {
            var hero = this.game.Players[playerIndex].Heroes.SingleOrDefault(h => h.HeroIndex == heroIndex);
            for (var i = 0; i < Constants.CreatureAmount; i++)
            {
                if (!hero.Creatures[i].Exist())
                {
                    hero.Creatures[i].Type = BitConverter.GetBytes(Constants.CreatureNames.IndexOf(creatureNameToAdd));
                    hero.Creatures[i].Amount = BitConverter.GetBytes(amountToAdd);
                    break;
                }
            }
            hero.Save(this.WriteMemory);
        }

        internal void SetAllResources(int playerIndex, int basicResourceAmount, int mithrilAmount)
        {
            for(var i = 0; i < Constants.BasicResourceTypeAmount; i++)
            {
                this.game.Players[playerIndex].SetBasicResource(i, BitConverter.GetBytes(basicResourceAmount));
            }
            this.game.Players[playerIndex].Mithril = BitConverter.GetBytes(mithrilAmount);
            this.game.Players[playerIndex].Save(this.WriteMemory);
        }

        internal List<Hero> GetHeroes(int playerIndex)
        {
            return this.game.Players[playerIndex].Heroes;
        }

        internal void SaveGame()
        {
            this.game.Save(this.WriteMemory);
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

        private bool IsAddressName(IntPtr address)
        {
            var memory = this.ReadMemory(address - Constants.ComputerNameMemoryOffset, 9);
            if (Constants.PlayerTypeNames.Any(name => name == memory.ToStringGBK()))
            {
                return true;
            }
            return false;
        }
    }
}
