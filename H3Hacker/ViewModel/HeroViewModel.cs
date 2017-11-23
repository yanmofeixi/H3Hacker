using H3Hacker.Model;

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

        public string Name
        {
            get
            {
                return this.hero.Name;
            }
        }

        public override string ToString()
        {
            return this.hero.Name;
        }
    }
}
