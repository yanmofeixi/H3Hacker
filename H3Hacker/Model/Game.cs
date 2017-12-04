using System;
using System.Collections.Generic;
using H3Hacker.GameSettings;

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

        internal override void Load(Func<IntPtr, uint, byte[]> readMemory)
        {
            this.Players = new List<Player>();
            this.Players = new List<Player>();
            for (var i = 0; i < PlayerAmount; i++)
            {
                var playerToAdd = new Player(
                    IntPtr.Add(this.BaseAddress, - PlayerMemoryOffset + Player.MemorySize * i), 
                    Constants.MithrilAddress + 4 * i);
                playerToAdd.Load(readMemory);
                this.Players.Add(playerToAdd);
            }

            var currentHeroAddress = this.BaseAddress;
            for (var i = 0; i < HeroTotalAmount; i++)
            {
                var playerIndex = readMemory(IntPtr.Add(currentHeroAddress, - 1), 1)[0];
                if (playerIndex != 0xFF) //hero exists
                {
                    var heroToAdd = new Hero(currentHeroAddress, playerIndex, i);
                    heroToAdd.Load(readMemory);
                    this.Players[heroToAdd.PlayerIndex].Heroes.Add(heroToAdd);
                }
                currentHeroAddress += Hero.MemorySize;
            }
        }

        internal override void Save(Action<IntPtr, byte[]> writeMemory)
        {
            for (var i = 0; i < PlayerAmount; i++)
            {
                this.Players[i].Save(writeMemory);
            }
        }
    }
}
