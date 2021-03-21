using H3Hacker.GameSettings;
using H3Hacker.Model;

namespace H3Hacker.ViewModel
{
    internal class ItemViewModel : ViewModelBase
    {
        private Item item;

        public ItemViewModel(Item item)
        {
            this.item = item;
        }

        public string Type
        {
            get
            {
                if (this.item.Exist() && Constants.ItemNames.Count > this.item.Type)
                {
                    return Constants.ItemNames[this.item.Type];
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                else
                {
                    this.item.Type = Constants.ItemNames.IndexOf(value);
                }
                this.OnPropertyChanged(nameof(this.Type));
            }
        }

        public bool Exist
        {
            get
            {
                return this.item.Exist();
            }
        }
    }
}
