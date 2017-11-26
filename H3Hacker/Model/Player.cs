using H3Hacker.GameSettings;
using H3Hacker.Utility;
using System.Collections.Generic;

namespace H3Hacker.Model
{
    internal class Player
    {
        internal List<Hero> Heroes = new List<Hero>();

        internal byte[] BasicResources = new byte[4 * Constants.BasicResourceTypeAmount];

        internal byte[] Mithril = new byte[4];

        internal byte[] GetBasicResource(int resourceIndex)
        {
            return this.BasicResources.GetSubBytes(4 * resourceIndex, 4);
        }

        internal void SetBasicResource(int resourceIndex, byte[] amount)
        {
            for (var i = 0; i < 4; i++)
            {
                this.BasicResources[4 * resourceIndex + i] = amount[i];
            }
        }
    }
}
