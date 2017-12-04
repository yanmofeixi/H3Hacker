using System.Collections.ObjectModel;
using System.Linq;
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

        public ObservableCollection<StatViewModel> Stats
        {
            get
            {
                var skills = new ObservableCollection<StatViewModel>();
                for (var i = 0; i < Hero.StatsAmount; i++)
                {
                    skills.Add(new StatViewModel(this.hero.Stats, i));
                }
                return skills;
            }
        }
    }
}
