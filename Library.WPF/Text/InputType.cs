using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismWPF.Control.Text
{
    //[Flags]
    //internal enum TextTypeOption
    //{
    //    None = 0,
    //    Email = 1 << 0,
    //    URL = 1 << 1,
    //    Number = 1 << 2,
    //    DecimalPoint = 1 << 3,
    //    PlusMinusSign = 1 << 4,
    //    Date = 1 << 5,
    //    Time = 1 << 6,
    //    All = ~(-1 << 7)
    //}

    //[Flags]
    //public enum TextType
    //{
    //    None = TextTypeOption.None,
    //    Email = TextTypeOption.Email,
    //    URL = TextTypeOption.URL,
    //    Number = TextTypeOption.Number,
    //    Decimal = TextTypeOption.Number | TextTypeOption.DecimalPoint,
    //    SignedNumber = TextTypeOption.Number | TextTypeOption.PlusMinusSign,
    //    SignedDecimal = TextTypeOption.Number | TextTypeOption.DecimalPoint | TextTypeOption.PlusMinusSign,
    //    DateTime = TextTypeOption.Date | TextTypeOption.Time,
    //    Date = TextTypeOption.Date,
    //    Time = TextTypeOption.Time,
    //    All = TextTypeOption.All
    //}

    [Flags]
    public enum TextType
    {
        None = 0,
        Number = 1 << 0,
        Decimal = 1 << 1,
        SignedNumber = 1 << 2,
        SignedDecimal = 1 << 3,
        DateTime = 1 << 4,
        Date = 1 << 5,
        Time = 1 << 6,
        Email = 1 << 7,
        URL = 1 << 8,
        Phone = 1 << 9,
        All = ~(-1 << 10),
        NullNumber = 11,
        PositiveInteger
    }
}
