﻿using H3Hacker.GameSettings;
using H3Hacker.Model;
using H3Hacker.Utility;
using System.Collections.ObjectModel;
using System.Text;

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
            return this.hero.Name.ToStringByEncoding(Encoding.GetEncoding(Constants.Encoding));
        }

        public short Mana
        {
            get
            {
                return this.hero.Mana;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > short.MaxValue)
                {
                    value = short.MaxValue;
                }
                this.hero.Mana = value;
            }
        }

        public int MovementPoint
        {
            get
            {
                return this.hero.MovementPoint;
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
                this.hero.MovementPoint = value;
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

        public ObservableCollection<CreatureViewModel> Creatures
        {
            get
            {
                var creatures = new ObservableCollection<CreatureViewModel>();
                for (var i = 0; i < Hero.MaximumCreatureType; i++)
                {
                    creatures.Add(new CreatureViewModel(this.hero.Creatures[i]));
                }
                return creatures;
            }
        }

        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                var items = new ObservableCollection<ItemViewModel>();
                for (var i = 0; i < Hero.ItemAmount; i++)
                {
                    items.Add(new ItemViewModel(this.hero.Items[i]));
                }
                return items;
            }
        }

        internal void RefreshDisplay()
        {
            this.OnPropertyChanged(nameof(this.Stats));
            this.OnPropertyChanged(nameof(this.BasicSkills));
            this.OnPropertyChanged(nameof(this.Creatures));
            this.OnPropertyChanged(nameof(this.Items));
        }
    }
}
