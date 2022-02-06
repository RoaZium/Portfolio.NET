namespace DMS.Module.Management.Model
{
    public class CompSelectionModel : SelectionModel
    {
        private string _CompName;
        public string CompName
        {
            get => _CompName;
            set
            {
                _CompName = value;
                RaisePropertyChanged("CompName");

                DisplayName = (CompName == null ? "" : CompName) +
                    (LayoutName == null ? "" : $" - {LayoutName}");
            }
        }

        private string _LayoutName;
        public string LayoutName
        {
            get => _LayoutName;
            set
            {
                _LayoutName = value;
                RaisePropertyChanged("LayoutName");

                DisplayName = (CompName == null ? "" : CompName) +
                    (LayoutName == null ? "" : $" - {LayoutName}");
            }
        }
    }
}
