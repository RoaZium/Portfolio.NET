using Prism.Mvvm;

namespace DMS.Module.Map.Model.Component
{
    public class AxisXRangeModel : BaseComponentModel
    {
        public override bool? AutoSideMargins { get => base.AutoSideMargins; set => base.AutoSideMargins = value; }
        public override bool? IsExpanded { get => base.IsExpanded; set => base.IsExpanded = value; }
        public override string SideMarginsValue { get => base.SideMarginsValue; set => base.SideMarginsValue = value; }
    }
}
