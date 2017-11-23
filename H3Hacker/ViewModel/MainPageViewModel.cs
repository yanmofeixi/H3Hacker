using H3Hacker.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace H3Hacker.ViewModel
{
    internal class MainPageViewModel : ViewModelBase
    {
        private int playerColorIndex = 0;

        private bool gameLoaded = false;

        public int PlayerColorIndex
        {
            get
            {
                return this.playerColorIndex;
            }

            set
            {
                this.playerColorIndex = value;
                this.OnPropertyChanged(nameof(PlayerColorIndex));
            }
        }

        public ObservableCollection<HeroViewModel> Heroes { get; set; } = new ObservableCollection<HeroViewModel>();

        public HeroViewModel SelectedHero { get; set; }

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

        public void AddHeroes(List<Hero> heroes)
        {
            this.Heroes.Clear();
            foreach (var hero in heroes)
            {
                this.Heroes.Add(new HeroViewModel(hero));
            }
            this.RefreshDisplay();
        }

        public void ClearHeroes()
        {
            this.Heroes.Clear();
            this.RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            this.OnPropertyChanged(nameof(Heroes));
        }
    }
}
