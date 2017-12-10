using H3Hacker.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace H3Hacker.ViewModel
{
    internal class MainPageViewModel : ViewModelBase
    {
        private int playerIndex = 0;

        private bool gameLoaded = false;

        private HeroViewModel selectedHeroViewModel;

        public int PlayerIndex
        {
            get
            {
                return this.playerIndex;
            }

            set
            {
                this.playerIndex = value;
                this.OnPropertyChanged(nameof(PlayerIndex));
            }
        }

        public ObservableCollection<HeroViewModel> Heroes { get; set; } = new ObservableCollection<HeroViewModel>();

        public HeroViewModel SelectedHero
        {
            get
            {
                return this.selectedHeroViewModel;
            }
            set
            {
                this.selectedHeroViewModel = value;
                this.OnPropertyChanged(nameof(SelectedHero));
            }
        }

        public bool GameLoaded
        {
            get
            {
                return this.gameLoaded;
            }

            set
            {
                this.gameLoaded = value;
                this.OnPropertyChanged(nameof(GameLoaded));
            }
        }

        internal void AddHeroes(List<Hero> heroes)
        {
            this.Heroes.Clear();
            foreach (var hero in heroes)
            {
                this.Heroes.Add(new HeroViewModel(hero));
            }
            this.RefreshDisplay();
        }

        internal void ClearHeroes()
        {
            this.Heroes.Clear();
            this.RefreshDisplay();
        }

        internal void RefreshDisplay()
        {
            this.OnPropertyChanged(nameof(Heroes));
            this.OnPropertyChanged(nameof(SelectedHero));
            foreach(var hero in this.Heroes)
            {
                hero.RefreshDisplay();
            }
        }
    }
}
