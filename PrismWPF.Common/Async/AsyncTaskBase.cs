using System.Windows.Threading;

namespace PrismWPF.Common.Async
{
    public abstract class AsyncTask
    {
        #region Properties
        private volatile AsyncTaskStatus p_Status;
        public AsyncTaskStatus Status
        {
            get => p_Status;
            protected set => p_Status = value;
        }

        private readonly Dispatcher _Dispatcher;
        public Dispatcher Dispatcher => _Dispatcher;

        public bool IsPending => Status == AsyncTaskStatus.PENDING;

        public bool IsRunning => Status == AsyncTaskStatus.RUNNING;

        public bool IsCancel => Status == AsyncTaskStatus.CANCELED || Status == AsyncTaskStatus.CANCELING;
        #endregion

        #region Constructor
        internal AsyncTask()
        {
            _Dispatcher = Dispatcher.CurrentDispatcher;

            Status = AsyncTaskStatus.PENDING;
        }
        #endregion

        #region Methods
        public void cancel()
        {
            if (Status != AsyncTaskStatus.PENDING)
            {
                Status = AsyncTaskStatus.CANCELING;
            }
        }
        #endregion
    }
}
