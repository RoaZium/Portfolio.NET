using System;
using System.Threading.Tasks;

namespace PrismWPF.Common.Async
{
    public enum AsyncTaskStatus
    {
        PENDING,
        RUNNING,
        CANCELING,
        CANCELED,
    }

    public class AsyncTask<ParamsType, ResultType> : AsyncTask
    {
        #region Properties
        public ResultType Result { get; private set; }
        #endregion

        #region Delegates
        private readonly Action<AsyncTask, ParamsType[]> _onPreExecute;
        private readonly Func<AsyncTask, ParamsType[], ResultType> _doInBackground;
        private readonly Action<AsyncTask, ResultType, ParamsType[]> _onPostExecute;
        private readonly Action<AsyncTask, ParamsType[]> _onCanceled;
        private readonly Action<AsyncTask, Exception, ParamsType[]> _onError;
        #endregion

        #region Constructor
        public AsyncTask(
            Action<AsyncTask, ParamsType[]> onPreExecute,
            Func<AsyncTask, ParamsType[], ResultType> doInBackground,
            Action<AsyncTask, ResultType, ParamsType[]> onPostExecute = null,
            Action<AsyncTask, ParamsType[]> onCanceled = null,
            Action<AsyncTask, Exception, ParamsType[]> onError = null)
            : base()
        {
            _onPreExecute = onPreExecute;
            _doInBackground = doInBackground;
            _onPostExecute = onPostExecute;
            _onCanceled = onCanceled;
            _onError = onError;
        }
        #endregion

        #region Methods
        public void execute(params ParamsType[] inputParams)
        {
            if (Status != AsyncTaskStatus.PENDING)
            {
                switch (Status)
                {
                    case AsyncTaskStatus.RUNNING:
                        throw new Exception("Cannot execute task:"
                            + " the task is already running.");
                    case AsyncTaskStatus.CANCELING:
                    case AsyncTaskStatus.CANCELED:
                        throw new Exception("Cannot execute task:"
                            + " the task is being canceling.");
                }
            }

            executeAsync(inputParams);
        }

        private async void executeAsync(params ParamsType[] inputParams)
        {
            try
            {
                Status = AsyncTaskStatus.RUNNING;
                Result = default(ResultType);

                _onPreExecute?.Invoke(this, inputParams);

                Exception e = null;
                await Task.Factory.StartNew(() =>
                {
                    try
                    {
                        if (_doInBackground != null)
                        {
                            Result = _doInBackground.Invoke(this, inputParams);
                        }
                    }
                    catch (Exception ex)
                    {
                        e = ex;
                    }
                });

                if (e != null)
                {
                    throw e;
                }

                if (Status == AsyncTaskStatus.RUNNING)
                {
                    _onPostExecute?.Invoke(this, Result, inputParams);
                }
                else if (Status == AsyncTaskStatus.CANCELING)
                {
                    Status = AsyncTaskStatus.CANCELED;
                    _onCanceled?.Invoke(this, inputParams);
                }
            }
            catch (Exception e)
            {
                _onError?.Invoke(this, e, inputParams);
            }
            finally
            {
                Status = AsyncTaskStatus.PENDING;
            }
        }
        #endregion
    }
}
