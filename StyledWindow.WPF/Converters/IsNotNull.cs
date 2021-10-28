using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace StyledWindow.WPF.Converters
{
    [ValueConversion(typeof(object), typeof(bool)), MarkupExtensionReturnType(typeof(IsNotNull))]
    internal class IsNotNull : ValueConverter
    {
        public override object Convert(object v, Type t, object p, CultureInfo c) => !(v is null);

        public override object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotSupportedException();

    }
}