
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace StyledWindow.WPF.Converters
{
    [ValueConversion(typeof(object), typeof(bool)), MarkupExtensionReturnType(typeof(IsNull))]
    internal class IsNull : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c) => v is null;

        public override object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotSupportedException();

    }
    [ValueConversion(typeof(IEnumerable), typeof(int)), MarkupExtensionReturnType(typeof(Any))]
    internal class Any : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c) => v is IEnumerable @enum ? @enum.Cast<object>().Any() : (object)null;
    }
}