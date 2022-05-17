using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using MathCore.WPF.Converters;
using MathCore.WPF.Converters.Base;

namespace StyledWindow.WPF.Converters
{
    [ValueConversion(typeof(IEnumerable), typeof(int)), MarkupExtensionReturnType(typeof(Any))]
    internal class Any : ValueConverter
    {
        protected override object Convert(object v, Type t, object p, CultureInfo c) => v is IEnumerable @enum ? @enum.Cast<object>().Any() : (object)null;
    }
}