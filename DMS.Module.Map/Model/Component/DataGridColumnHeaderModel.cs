using DMS.Module.Map.Model.Component;

namespace DMS.Module.Map.Model.DataGrid
{
    public class DataGridColumnHeaderModel : BaseComponentModel
    {
        public override string Background { get => base.Background; set => base.Background = value; }
        public override string BorderBrush { get => base.BorderBrush; set => base.BorderBrush = value; }
        public override int? BorderThickness { get => base.BorderThickness; set => base.BorderThickness = value; }
        public override string FontSize { get => base.FontSize; set => base.FontSize = value; }
        public override string FontStyle { get => base.FontStyle; set => base.FontStyle = value; }
        public override string FontWeight { get => base.FontWeight; set => base.FontWeight = value; }
        public override string Foreground { get => base.Foreground; set => base.Foreground = value; }
        public override string Height { get => base.Height; set => base.Height = value; }
        public override string HorizontalContentAlignment { get => base.HorizontalContentAlignment; set => base.HorizontalContentAlignment = value; }
        public override bool? IsExpanded { get => base.IsExpanded; set => base.IsExpanded = value; }
        public override string VerticalContentAlignment { get => base.VerticalContentAlignment; set => base.VerticalContentAlignment = value; }
    }
}
