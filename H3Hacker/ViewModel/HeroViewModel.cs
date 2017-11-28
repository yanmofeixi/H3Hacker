using System.Collections.ObjectModel;
using H3Hacker.GameSettings;
using H3Hacker.Model;
using H3Hacker.Utility;

namespace H3Hacker.ViewModel
{
    internal class HeroViewModel : ViewModelBase
    {
        private Hero hero;

        internal HeroViewModel(Hero hero)
        {
            this.hero = hero;
        }

        internal int HeroIndex
        {
            get
            {
                return this.hero.HeroIndex;
            }
        }

        internal int PlayerIndex
        {
            get
            {
                return this.hero.PlayerIndex;
            }
        }

        public int Attack
        {
            get
            {
                return this.hero.GetStat(0);
            }
            set
            {
                this.hero.SetStat(0, value);
                this.OnPropertyChanged(nameof(Attack));
            }
        }

        public int Defence
        {
            get
            {
                return this.hero.GetStat(1);
            }
            set
            {
                this.hero.SetStat(1, value);
                this.OnPropertyChanged(nameof(Defence));
            }
        }

        public override string ToString()
        {
            return this.hero.Name.ToStringGBK();
        }

        public ObservableCollection<BasicSkillViewModel> BasicSkills
        {
            get
            {
                var skills = new ObservableCollection<BasicSkillViewModel>();
                for (var i = 0; i < Hero.BasicSkillAmount; i++)
                {
                    skills.Add(new BasicSkillViewModel(this.hero.BasicSkills, i));
                }
                return skills;
            }
        }

        public ObservableCollection<byte> Stats;
    }
}
