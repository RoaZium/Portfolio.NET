using DMS.Module.Map.Model.Component;

namespace DMS.Module.Map.Model.DataGrid
{
    public class DataGridRowModel : BaseComponentModel
    {
        public override string Height { get => base.Height; set => base.Height = value; }
        public override bool? IsExpanded { get => base.IsExpanded; set => base.IsExpanded = value; }
    }
}
