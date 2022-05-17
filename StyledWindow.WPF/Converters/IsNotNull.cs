using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using MathCore.WPF.Converters;
using MathCore.WPF.Converters.Base;

namespace StyledWindow.WPF.Converters
{
    [ValueConversion(typeof(object), typeof(bool)), MarkupExtensionReturnType(typeof(IsNotNull))]
    internal class IsNotNull : ValueConverter
    {
        protected override object Convert(object v, Type t, object p, CultureInfo c) => !(v is null);

        protected override object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotSupportedException();

    }
}