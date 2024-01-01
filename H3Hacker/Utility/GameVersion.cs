namespace H3Hacker.Utility
{
    internal static class GameVersion
    {
        public enum GameVersionType
        {
            Unknown = 0,
            H3Era = 1,
            VCMI = 2
        }

        public static GameVersionType gameVersion = GameVersionType.Unknown;
    }
}
