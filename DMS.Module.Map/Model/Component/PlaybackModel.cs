namespace DMS.Module.Map.Model.Component
{
    public class PlaybackModel : BaseComponentModel
    {
        public override bool? Play { get => base.Play; set => base.Play = value; }
        public override string Rotation { get => base.Rotation; set => base.Rotation = value; }
        public override string Url { get => base.Url; set => base.Url = value; }
    }
}
