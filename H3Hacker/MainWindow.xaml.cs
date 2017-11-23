using System.Windows;
using H3Hacker.Memory;
using H3Hacker.ViewModel;

namespace H3Hacker
{
    public partial class MainWindow : Window
    {
        private MainPageViewModel mainPageViewModel = new MainPageViewModel();

        private GameMemoryManager gameMemoryManager = new GameMemoryManager();

        public MainWindow()
        {
            InitializeComponent();
            this.MainWindowPanel.DataContext = this.mainPageViewModel;
        }

        private void Initialize()
        {
            if (this.gameMemoryManager.OpenProcess())
            {
                this.mainPageViewModel.AddHeroes(this.gameMemoryManager.GetHeroes(this.mainPageViewModel.PlayerColorIndex));
                this.mainPageViewModel.GameLoaded = true;
            }
            else
            {
                this.mainPageViewModel.ClearHeroes();
                this.mainPageViewModel.GameLoaded = false;
                MessageBox.Show("读取失败");
            }
        }

        private void Load_OnClick(object sender, RoutedEventArgs e)
        {
            this.Initialize();
        }

        private void Resource_OnClick(object sender, RoutedEventArgs e)
        {
            this.gameMemoryManager.MaxAllResources(this.mainPageViewModel.PlayerColorIndex);
        }

        private void Commander_OnClick(object sender, RoutedEventArgs e)
        {
            this.gameMemoryManager.ModifyCommander(this.mainPageViewModel.SelectedHero.Index);
        }

        private void Creature_OnClick(object sender, RoutedEventArgs e)
        {
            this.gameMemoryManager.AddCreature(this.mainPageViewModel.SelectedHero.Index);
        }

        private void PlayerColor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.Initialize();
        }
    }
}
