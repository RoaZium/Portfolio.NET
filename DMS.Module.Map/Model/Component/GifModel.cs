using Newtonsoft.Json;

namespace DMS.Module.Map.Model.Component
{
    public class GifModel : BaseComponentModel
    {
        public override double? AnimationSpeedRatio { get => base.AnimationSpeedRatio; set => base.AnimationSpeedRatio = value; }
        [JsonIgnore]
        public override string FilePath { get => base.FilePath; set => base.FilePath = value; }
        public override bool? IsExpanded { get => base.IsExpanded; set => base.IsExpanded = value; }
        public override string Name { get => base.Name; set => base.Name = value; }
        public override bool? Play { get => base.Play; set => base.Play = value; }
    }
}
