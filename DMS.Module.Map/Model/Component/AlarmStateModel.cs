namespace DMS.Module.Map.Model.Component
{
    public class AlarmStateModel : BaseComponentModel
    {
        public override bool? BackgroundAlarmVisibility { get => base.BackgroundAlarmVisibility; set => base.BackgroundAlarmVisibility = value; }
        public override bool? BorderAlarmVisibility { get => base.BorderAlarmVisibility; set => base.BorderAlarmVisibility = value; }
        public override string Fill { get => base.Fill; set => base.Fill = value; }
        public override bool? IsExpanded { get => base.IsExpanded; set => base.IsExpanded = value; }
        public override bool? NormalAlarmVisibility { get => base.NormalAlarmVisibility; set => base.NormalAlarmVisibility = value; }
        public override string Opacity { get => base.Opacity; set => base.Opacity = value; }
        public override string RadiusX { get => base.RadiusX; set => base.RadiusX = value; }
        public override string RadiusY { get => base.RadiusY; set => base.RadiusY = value; }
        public override string Stroke { get => base.Stroke; set => base.Stroke = value; }
        public override string StrokeThickness { get => base.StrokeThickness; set => base.StrokeThickness = value; }
        public override string Visibility { get => base.Visibility; set => base.Visibility = value; }
    }
}
