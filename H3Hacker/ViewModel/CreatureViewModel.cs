using H3Hacker.GameSettings;
using H3Hacker.Model;

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
                    return Constants.CreatureNames[this.creature.Type];
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
                    this.OnPropertyChanged(nameof(this.Amount));
                }
                else
                {
                    this.creature.Type = Constants.CreatureNames.IndexOf(value);
                }
                this.OnPropertyChanged(nameof(this.Type));
            }
        }

        public int Amount
        {
            get
            {
                return this.creature.Amount;
            }
            set
            {
                if(value < 0)
                {
                    value = 0;
                }
                this.creature.Amount = value;
                this.OnPropertyChanged(nameof(this.Amount));
            }
        }
    }
}
