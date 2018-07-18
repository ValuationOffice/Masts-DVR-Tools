using masts_dvr_tool.Types.Helpers;

namespace masts_dvr_tool.Types
{
    public class PDFField : BindableBase
    {
        private string name;
        private string value;

        public string Name
        {
            get => this.name;
            set
            {
                if (value == this.name)
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged();
            }
        }
        public string Value
        {
            get => this.value;
            set
            {
                if (value == this.value)
                {
                    return;
                }

                this.value = value;
                this.OnPropertyChanged();
            }
        }
    }
}
