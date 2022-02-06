using DMS.Module.Map.Model.Component;

namespace DMS.Module.Map.Model.DataGrid
{
    public class DataGridModel : BaseComponentModel
    {
        public override string Background { get => base.Background; set => base.Background = value; }
        public override bool? IsExpanded { get => base.IsExpanded; set => base.IsExpanded = value; }
    }
}
