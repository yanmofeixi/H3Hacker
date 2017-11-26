using System.Windows;
using H3Hacker.Memory;
using H3Hacker.ViewModel;
using System.Collections.Generic;

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
                this.mainPageViewModel.AddHeroes(this.gameMemoryManager.GetHeroes(this.mainPageViewModel.PlayerIndex));
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
            var basicResourceAmount = 99999999;
            var mithrilAmount = 999999;
            this.gameMemoryManager.SetAllResources(
                this.mainPageViewModel.PlayerIndex,
                basicResourceAmount,
                mithrilAmount);
        }

        private void Commander_OnClick(object sender, RoutedEventArgs e)
        {
            var itemsToAdd = new List<string>
            {
                "击碎之斧",
                "秘银之甲",
                "锋利之剑",
                "不朽之冠",
                "加速之靴",
                "硬化之盾"
            };
            var basicSkillLevel = 1;
            this.gameMemoryManager.ModifyCommander(
                this.mainPageViewModel.SelectedHero.HeroIndex, 
                itemsToAdd,
                basicSkillLevel);
        }

        private void Creature_OnClick(object sender, RoutedEventArgs e)
        {
            var creatureToAdd = "幽灵比蒙";
            var amountToAdd = 1;
            this.gameMemoryManager.AddCreature(
                this.mainPageViewModel.SelectedHero.HeroIndex,
                this.mainPageViewModel.SelectedHero.PlayerIndex,
                creatureToAdd,
                amountToAdd);
        }

        private void PlayerIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.Initialize();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            this.gameMemoryManager.SaveGame();
        }
    }
}
