using System.Collections.ObjectModel;
using System.Linq;
using H3Hacker.Model;
using H3Hacker.Utility;
using System;

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

        public short Mana
        {
            get
            {
                return BitConverter.ToInt16(this.hero.Mana, 0);
            }
            set
            {
                if(value < 0)
                {
                    value = 0;
                }
                else if(value > short.MaxValue)
                {
                    value = short.MaxValue;
                }
                value.CopyToByteArray(this.hero.Mana, 0);
            }
        }

        public int MovementPoint
        {
            get
            {
                return BitConverter.ToInt32(this.hero.MovementPoint, 0);
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > int.MaxValue)
                {
                    value = int.MaxValue;
                }
                value.CopyToByteArray(this.hero.MovementPoint, 0);
            }
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
