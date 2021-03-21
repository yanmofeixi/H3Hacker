using System;
using System.Collections.Generic;
using System.Windows;
using H3Hacker.GameMemory;
using H3Hacker.ViewModel;

namespace H3Hacker
{
    public partial class MainWindow : Window, IDisposable
    {
        private MainPageViewModel mainPageViewModel = new MainPageViewModel();

        private GameMemoryManager gameMemoryManager = new GameMemoryManager();

        public MainWindow()
        {
            this.InitializeComponent();
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
                MessageBox.Show("读取失败，请确保h3era.exe正在运行");
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
            this.mainPageViewModel.RefreshDisplay();
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
                this.mainPageViewModel.SelectedHero.PlayerIndex,
                itemsToAdd,
                basicSkillLevel);
            this.mainPageViewModel.RefreshDisplay();
        }

        private void Creature_OnClick(object sender, RoutedEventArgs e)
        {
            var creatureToAdd = "幽冥比蒙";
            var amountToAdd = 1;
            this.gameMemoryManager.AddCreature(
                this.mainPageViewModel.SelectedHero.HeroIndex,
                this.mainPageViewModel.SelectedHero.PlayerIndex,
                creatureToAdd,
                amountToAdd);
            this.mainPageViewModel.RefreshDisplay();
        }

        private void PlayerIndexChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            this.Initialize();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            this.gameMemoryManager.SaveGame();
        }

        public void Dispose()
        {
            this.gameMemoryManager.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
