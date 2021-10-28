using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace StyledWindow.WPF.Converters
{
    [MarkupExtensionReturnType(typeof(ValueConverter))]
    internal abstract class ValueConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider sp) => this;

        public abstract object Convert(object v, [NotNull] Type t, object p, [NotNull] CultureInfo c);

        public virtual object ConvertBack(object v, [NotNull] Type t, object p, [NotNull] CultureInfo c) => throw new NotSupportedException();
    }
}