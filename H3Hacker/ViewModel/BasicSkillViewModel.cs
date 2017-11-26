namespace H3Hacker.ViewModel
{
    internal class BasicSkillViewModel : ViewModelBase
    {
        public string Name { get; set; }

        public string Level { get; set; }

        public override string ToString()
        {
            return this.Name + this.Level;
        }
    }
}
