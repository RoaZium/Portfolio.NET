using System;

namespace PrismWPF.Common.Events
{
    public class ValueChangedEventArgs<T> : EventArgs
    {
        private readonly T _Value;
        public T Value => _Value;

        public ValueChangedEventArgs(T value)
        {
            _Value = value;
        }
    }
}
