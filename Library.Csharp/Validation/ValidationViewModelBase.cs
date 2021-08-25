using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrismWPF.Common.Validation
{
    public abstract class ValidationViewModelBase : BindableBase, INotifyDataErrorInfo
    {
        #region Validation
        private bool IsFirst = true;

        private readonly ConcurrentDictionary<string, List<string>> _errors = new ConcurrentDictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private readonly object _lock = new object();

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            //ValidateAsync();
        }

        public void OnErrorsChanged(string propertyName)
        {
            EventHandler<DataErrorsChangedEventArgs> handler = ErrorsChanged;
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            _errors.TryGetValue(propertyName, out List<string> errorsForName);
            return errorsForName;
        }

        public List<KeyValuePair<string, List<string>>> GetErrors()
        {
            return _errors.Where(kv => kv.Value != null && kv.Value.Count > 0).ToList();
        }

        [JsonIgnore]
        public bool HasErrors
        {
            get
            {
                if (IsFirst)
                {
                    IsFirst = false;
                    //ValidateAsync();
                }
                return _errors.Any(kv => kv.Value != null && kv.Value.Count > 0);
            }
        }

        public Task ValidateAsync()
        {
            return Task.Run(() => Validate());
        }

        public void Validate()
        {
            lock (_lock)
            {
                ValidationContext validationContext = new ValidationContext(this, null, null);
                List<ValidationResult> validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(this, validationContext, validationResults, true);

                foreach (KeyValuePair<string, List<string>> kv in _errors.ToList())
                {
                    if (validationResults.All(r => r.MemberNames.All(m => m != kv.Key)))
                    {
                        _errors.TryRemove(kv.Key, out List<string> outLi);
                        OnErrorsChanged(kv.Key);
                    }
                }

                IEnumerable<IGrouping<string, ValidationResult>> q = from r in validationResults
                                                                     from m in r.MemberNames
                                                                     group r by m into g
                                                                     select g;

                foreach (IGrouping<string, ValidationResult> prop in q)
                {
                    List<string> messages = prop.Select(r => r.ErrorMessage).ToList();

                    if (_errors.ContainsKey(prop.Key))
                    {
                        _errors.TryRemove(prop.Key, out List<string> outLi);
                    }
                    _errors.TryAdd(prop.Key, messages);
                    OnErrorsChanged(prop.Key);
                }
            }
        }
        #endregion
    }
}
