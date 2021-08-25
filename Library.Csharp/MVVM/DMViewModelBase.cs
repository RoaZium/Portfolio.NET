using Newtonsoft.Json;
using Prism.Events;
using Prism.Regions;
using PrismWPF.Common.Validation;
using System;
using System.Windows;
using Unity;

namespace PrismWPF.Common.MVVM
{
    public class DMViewModelBase : ValidationViewModelBase, IDisposable
    {
        private bool isDisposed;

        private IUnityContainer _container;
        private IEventAggregator _eventAggregator;
        private RegionManager _regionManager;
        private UIElement _uiElement;

        public DMViewModelBase()
        {
        }

        public virtual void Load()
        {
        }

        public virtual void Unload()
        {

        }

        public virtual void SubscribeEvent()
        {

        }

        public virtual void UnsubscribeEvent()
        {

        }

        [JsonIgnore]
        public IUnityContainer Container
        {
            get => _container;
            set => _container = value;
        }

        [JsonIgnore]
        public virtual IEventAggregator EventAggregator
        {
            get => _eventAggregator;
            set => _eventAggregator = value;
        }

        [JsonIgnore]
        public RegionManager RegionManager
        {
            get => _regionManager;
            set => _regionManager = value;
        }

        [JsonIgnore]
        public UIElement OwnerView
        {
            get => _uiElement;
            set => _uiElement = value;
        }

        #region DisplayName

        /// <summary>
        /// Returns the user-friendly name of this object.
        /// Child classes can set this property to a new value,
        /// or override it to determine the value on-demand.
        /// </summary>
        [JsonIgnore]
        public virtual string DisplayName { get; protected set; }

        #endregion // DisplayName

        #region IDisposable Members

        /// <summary>
        /// Invoked when this object is being removed from the application
        /// and will be subject to garbage collection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Child classes can override this method to perform 
        /// clean-up logic, such as removing event handlers.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    //Managed
                    DisposeManaged();
                }

                //Unmanaged
                DisposeUnmanaged();

                isDisposed = true;
            }
        }

        protected virtual void DisposeManaged()
        {

        }

        protected virtual void DisposeUnmanaged()
        {

        }

        #endregion

        #region 리팩토링 대상

        public event EventHandler RequestClose;

        public void OnCultureChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged("Name");
        }

        public void Close()
        {
            EventHandler handler = RequestClose;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
