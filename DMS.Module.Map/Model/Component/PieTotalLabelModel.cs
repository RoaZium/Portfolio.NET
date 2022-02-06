using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Module.Map.Model.Component
{
    public class PieTotalLabelModel : BaseComponentModel
    {
        public override string FontSize { get => base.FontSize; set => base.FontSize = value; }
        public override string FontStyle { get => base.FontStyle; set => base.FontStyle = value; }
        public override string FontWeight { get => base.FontWeight; set => base.FontWeight = value; }
        public override string Foreground { get => base.Foreground; set => base.Foreground = value; }
        public override bool? IsExpanded { get => base.IsExpanded; set => base.IsExpanded = value; }
        public override string MaxWidth { get => base.MaxWidth; set => base.MaxWidth = value; }
        public override string TextPattern { get => base.TextPattern; set => base.TextPattern = value; }
        public override string Visibility { get => base.Visibility; set => base.Visibility = value; }
    }
}
