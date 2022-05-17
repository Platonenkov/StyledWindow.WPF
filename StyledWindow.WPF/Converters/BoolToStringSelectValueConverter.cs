using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using MathCore.WPF.Converters;
using MathCore.WPF.Converters.Base;

namespace StyledWindow.WPF.Converters
{
    /// <summary>
    /// Конвертер значения bool в текст
    /// задайте в параметрах значение для True и False
    /// </summary>
    [ValueConversion(typeof(bool), typeof(string)), MarkupExtensionReturnType(typeof(BoolToStringSelectValueConverter))]
    internal class BoolToStringSelectValueConverter : ValueConverter
    {
        /// <summary>Возвращаемое значение при Истина</summary>
        public string True
        {
            private get => _True;
            set => _True = value;

        }
        private string _True;

        /// <summary>Возвращаемое значение при Лож</summary>
        public string False
        {
            private get => _False;
            set => _False = value;

        }
        private string _False;

        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value != null && ((bool)value) ? True : False;

        protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}
