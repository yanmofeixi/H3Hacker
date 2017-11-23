using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using H3Hacker.Model;
using H3Hacker.GameSettings;

namespace H3Hacker.Utility
{
    internal class GameMemoryManager
    {
        private ProcessMemoryReaderApi.SYSTEM_INFO systemInformation;

        private Process gameProcess;

        private IntPtr handle;

        private List<Hero> heroes;

        private IntPtr baseAddress;

        internal GameMemoryManager()
        {
            ProcessMemoryReaderApi.GetSystemInfo(out this.systemInformation);
        }

        internal bool OpenProcess()
        {
            this.gameProcess = Process.GetProcesses().FirstOrDefault(p => p.ProcessName == "h3era");

            if(this.gameProcess == null)
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

        internal void ModifyCommander(int heroIndex)
        {
            var commanderAddress = Constants.CommanderBaseAddress + heroIndex * Constants.CommanderMemorySize;
            var level = BitConverter.GetBytes(1);
            var skills = new byte[28];
            for(var i = 0; i < 7; i++)
            {
                skills[4 * i] = level[0];
                skills[4 * i + 1] = level[1];
                skills[4 * i + 2] = level[2];
                skills[4 * i + 3] = level[3];
            }
            this.WriteMemory(commanderAddress - 0xBC, skills);
            var itemsToAdd = new List<string>
            {
                "击碎之斧",
                "秘银之甲",
                "锋利之剑",
                "不朽之冠",
                "加速之靴",
                "硬化之盾"
            };

            for(var i = 0; i < 6; i++)
            {
                var itemIndex = (short) Constants.CommanderItems.IndexOf(itemsToAdd[i]);
                var itemBytes = BitConverter.GetBytes((short)(itemIndex + 0x92));
                this.WriteMemory(commanderAddress - 0xA0 + 0x10 * i, itemBytes);
            }
        }

        internal void MaxAllResources(int playerColorIndex)
        {
            var max = BitConverter.GetBytes(Constants.MaxResourceAmount);
            var resourceBaseAddress = this.baseAddress 
                - Constants.PlayerMemoryOffset 
                + Constants.PlayerMemorySize * playerColorIndex;
            var resources = new byte[28];
            for(var i = 0; i < 7; i++)
            {
                resources[4 * i] = max[0];
                resources[4 * i + 1] = max[1];
                resources[4 * i + 2] = max[2];
                resources[4 * i + 3] = max[3];
            }
            this.WriteMemory(resourceBaseAddress, resources);
            this.WriteMemory(Constants.MithrilAddress + (4 * playerColorIndex), new byte[] { 0x3F, 0x42, 0x0F });
        }

        internal List<Hero> GetHeroes(int playerColor)
        {
            return this.heroes.Where(h => h.Color == playerColor).ToList();
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
                    this.heroes.Add(new Hero {
                        Name = this.ReadMemory(currentHeroAddress, 12).ToStringGBK(),
                        Color = this.ReadMemory(currentHeroAddress - 1, 1)[0],
                        Address = currentHeroAddress,
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
            if(Constants.PlayerTypeNames.Any(name => name == memory.ToStringGBK()))
            {
                return true;
            }
            return false;
        }
    }
}
