using H3Hacker.GameSettings;

namespace H3Hacker.ViewModel
{
    internal class StatViewModel : ViewModelBase
    {
        private int statIndex;

        private byte[] stats;

        public StatViewModel(byte[] stats, int statIndex)
        {
            this.stats = stats;
            this.statIndex = statIndex;
        }

        public string Name
        {
            get
            {
                return Constants.StatsNames[this.statIndex];
            }
        }

        public byte StatPoint
        {
            get
            {
                return this.stats[this.statIndex];
            }
            set
            {
                if(value > sbyte.MaxValue)
                {
                    value = (byte)sbyte.MaxValue;
                }
                else if(value < 0)
                {
                    value = 0;
                }
                this.stats[this.statIndex] = value;
                this.OnPropertyChanged(nameof(StatPoint));
            }
        }
    }
}
