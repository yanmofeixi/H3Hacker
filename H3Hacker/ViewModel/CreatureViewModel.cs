using System;
using H3Hacker.GameSettings;
using H3Hacker.Model;
using H3Hacker.Utility;

namespace H3Hacker.ViewModel
{
    internal class CreatureViewModel : ViewModelBase
    {
        private Creature creature;

        public CreatureViewModel(Creature creature)
        {
            this.creature = creature;
        }

        public string Type
        {
            get
            {
                if (this.creature.Exist())
                {
                    return Constants.CreatureNames[BitConverter.ToInt32(this.creature.Type, 0)];
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.creature.Remove();
                    this.OnPropertyChanged(nameof(Amount));
                }
                else
                {
                    var index = Constants.CreatureNames.IndexOf(value);
                    index.CopyToByteArray(this.creature.Type, 0);
                }
                this.OnPropertyChanged(nameof(Type));
            }
        }

        public int Amount
        {
            get
            {
                var a = BitConverter.ToInt32(this.creature.Amount, 0);
                return a;// BitConverter.ToInt32(this.creature.Amount, 0);
            }
            set
            {
                if(value < 0)
                {
                    value = 0;
                }
                value.CopyToByteArray(this.creature.Amount, 0);
                this.OnPropertyChanged(nameof(Amount));
            }
        }
    }
}
