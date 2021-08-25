using System;

namespace PrismWPF.Common.Events
{
    public class ValueChangingEventArgs<T> : EventArgs
    {
        private readonly T _OldValue;
        public T OldValue => _OldValue;

        private readonly T _NewValue;
        public T NewValue => _NewValue;

        private bool _Cancel = false;
        public bool Cancel
        {
            get => _Cancel;
            set => _Cancel = value;
        }

        public ValueChangingEventArgs(T oldValue, T newValue)
        {
            _OldValue = oldValue;
            _NewValue = newValue;
        }
    }
}
