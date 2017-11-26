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

        internal int Index
        {
            get
            {
                return this.hero.Index;
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
                for (var i = 0; i < Constants.HeroBasicSkillAmount; i++)
                {
                    skills.Add(new BasicSkillViewModel
                    {
                        Name = Constants.BasicSkillNames[i],
                        Level = Constants.BasicSkillLevelNames[this.hero.BasicSkills[i]]                     
                    });
                }
                return skills;
            }
            set
            {

            }
        }
    }
}
