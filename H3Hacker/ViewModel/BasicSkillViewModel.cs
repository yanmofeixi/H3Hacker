using H3Hacker.GameSettings;

namespace H3Hacker.ViewModel
{
    internal class BasicSkillViewModel : ViewModelBase
    {
        private int skillIndex;

        private byte[] basicSkills;

        public BasicSkillViewModel(byte[] basicSkills, int skillIndex)
        {
            this.basicSkills = basicSkills;
            this.skillIndex = skillIndex;
        }

        public string Name
        {
            get
            {
                return Constants.BasicSkillNames[this.skillIndex];
            }
        }

        public string Level
        {
            get
            {
                return Constants.BasicSkillLevelNames[this.basicSkills[this.skillIndex]];
            }
            set
            {
                this.basicSkills[this.skillIndex] = (byte)Constants.BasicSkillLevelNames.IndexOf(value);
                this.OnPropertyChanged(nameof(this.Level));
            }
        }
    }
}
