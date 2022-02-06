namespace DMS.Module.Map.Model.Component
{
    public class AxisYRangeModel : BaseComponentModel
    {
        public override bool? AutoSideMargins { get => base.AutoSideMargins; set => base.AutoSideMargins = value; }
        public override bool? IsExpanded { get => base.IsExpanded; set => base.IsExpanded = value; }
        public override string MaxValue { get => base.MaxValue; set => base.MaxValue = value; }
        public override string MinValue { get => base.MinValue; set => base.MinValue = value; }
        public override string SideMarginsValue { get => base.SideMarginsValue; set => base.SideMarginsValue = value; }
    }
}
