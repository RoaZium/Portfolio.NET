
using Prism.Commands;
using System;
using System.Windows.Controls;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_VideoViewMode : CP_CommonBaseViewModel
    {
        protected override void OnComponentData()
        {

        }

        public DelegateCommand<MediaElement> MediaEndedCommand => new DelegateCommand<MediaElement>(MediaEndedCommandExecute);

        private void MediaEndedCommandExecute(MediaElement sender)
        {
            if (sender != null && sender is MediaElement)
            {
                sender.Position = TimeSpan.FromSeconds(0);
            }
        }
    }
}
