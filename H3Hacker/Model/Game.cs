using H3Hacker.GameSettings;
using H3Hacker.Utility;
using System;
using System.Collections.Generic;

namespace H3Hacker.Model
{
    internal class Game : MemoryObject
    {
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
            for (var i = 0; i < Constants.PlayerAmount; i++)
            {
                var playerToAdd = new Player(
                    this.BaseAddress - Constants.PlayerMemoryOffset + Constants.PlayerMemorySize * i, 
                    Constants.MithrilAddress + 4 * i);
                playerToAdd.Load(readMemory);
                this.Players.Add(playerToAdd);
            }

            var currentHeroAddress = this.BaseAddress;
            for (var i = 0; i < Constants.HeroTotalAmount; i++)
            {
                var playerIndex = readMemory(currentHeroAddress - 1, 1)[0];
                if (playerIndex != 0xFF) //hero exists
                {
                    var heroToAdd = new Hero(currentHeroAddress, playerIndex, i);
                    heroToAdd.Load(readMemory);
                    this.Players[heroToAdd.PlayerIndex].Heroes.Add(heroToAdd);
                }
                currentHeroAddress += Constants.HeroMemorySize;
            }
        }

        internal override void Save(Action<IntPtr, byte[]> writeMemory)
        {
            for (var i = 0; i < Constants.PlayerAmount; i++)
            {
                this.Players[i].Save(writeMemory);
            }
        }
    }
}
