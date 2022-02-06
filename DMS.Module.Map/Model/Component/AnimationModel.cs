namespace DMS.Module.Map.Model.Component
{
    public class AnimationModel : BaseComponentModel
    {
        public override string Background { get => base.Background; set => base.Background = value; }
        public override bool? BackgroundAlarmVisibility { get => base.BackgroundAlarmVisibility; set => base.BackgroundAlarmVisibility = value; }
        public override string BorderBrush { get => base.BorderBrush; set => base.BorderBrush = value; }
        public override int? BorderThickness { get => base.BorderThickness; set => base.BorderThickness = value; }
        public override bool? BorderAlarmVisibility { get => base.BorderAlarmVisibility; set => base.BorderAlarmVisibility = value; }
        public override string Brush { get => base.Brush; set => base.Brush = value; }
        public override bool? BrushAlarmVisibility { get => base.BrushAlarmVisibility; set => base.BrushAlarmVisibility = value; }
        public override string CornerRadius { get => base.CornerRadius; set => base.CornerRadius = value; }
        public override bool? IsExpanded { get => base.IsExpanded; set => base.IsExpanded = value; }
        public override string Opacity { get => base.Opacity; set => base.Opacity = value; }
    }
}
