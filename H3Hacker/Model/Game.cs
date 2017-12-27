using System;
using System.Collections.Generic;
using H3Hacker.GameSettings;
using ProcessMemoryScanner;

namespace H3Hacker.Model
{
    internal class Game : MemoryObject
    {
        private const int HeroTotalAmount = 156;

        private const int PlayerAmount = 8;

        private const int PlayerMemoryOffset = 0x00000AD7;

        internal Game(IntPtr baseAddress) : base(baseAddress)
        {
        }

        internal bool IsAddressValid
        {
            get
            {
                return this.BaseAddress != IntPtr.Zero;
            }
        }

        internal List<Player> Players;

        internal override void Load(MemoryScanner memory)
        {
            this.Players = new List<Player>();
            for (var i = 0; i < PlayerAmount; i++)
            {
                var playerToAdd = new Player(
                    this.BaseAddress - PlayerMemoryOffset + Player.MemorySize * i, 
                    Constants.MithrilAddress + 4 * i);
                playerToAdd.Load(memory);
                this.Players.Add(playerToAdd);
            }

            var currentHeroAddress = this.BaseAddress;
            for (var i = 0; i < HeroTotalAmount; i++)
            {
                var playerIndex = memory.ReadMemory<byte>(currentHeroAddress - 1);
                if (playerIndex != 0xFF) //hero exists
                {
                    var heroToAdd = new Hero(currentHeroAddress, playerIndex, i);
                    heroToAdd.Load(memory);
                    this.Players[heroToAdd.PlayerIndex].Heroes.Add(heroToAdd);
                }
                currentHeroAddress += Hero.MemorySize;
            }
        }

        internal override void Save(MemoryScanner memory)
        {
            for (var i = 0; i < PlayerAmount; i++)
            {
                this.Players[i].Save(memory);
            }
        }
    }
}
