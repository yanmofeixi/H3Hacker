using H3Hacker.GameSettings;
using H3Hacker.Utility;
using System;
using System.Collections.Generic;

namespace H3Hacker.Model
{
    internal class Hero
    {
        internal IntPtr Address;

        internal byte[] BasicSkills;

        internal List<Creature> Creatures = new List<Creature>();

        internal int HeroIndex;

        internal byte[] Name;

        internal int PlayerIndex;

        internal byte[] GetCreatures()
        {
            var result = new byte[4 * 2 * Constants.CreatureAmount];
            for (var i = 0; i < Constants.CreatureAmount; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    result[4 * i + j] = this.Creatures[i].Type[j];
                    result[4 * i + 4 * Constants.CreatureAmount + j] = this.Creatures[i].Amount[j];
                }
            }
            return result;
        }

        internal void SetCreatures(int index, byte[] type, byte[] amount)
        {
            for (var i = 0; i < Constants.CreatureAmount; i++)
            {
                this.Creatures.Add(new Creature
                {
                    Type = type.GetSubBytes(4 * i, 4),
                    Amount = amount.GetSubBytes(4 * i, 4)
                });
            }
        }
    }
}
