using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;
using MathCore.Annotations;

namespace TestApp.WPF
{
    public class EnumDescriptions : MarkupExtension
    {
        private Type _Type;

        public Type Type
        {
            get => _Type;
            set
            {
                if (value != null && !value.IsEnum) throw new ArgumentException("Тип не является перечислением", nameof(value));
                _Type = value;
            }
        }

        public enum NullValueLocation { None, First, Last }

        public NullValueLocation NullValue { get; set; } = NullValueLocation.None;

        public EnumDescriptions() { }

        public EnumDescriptions([NotNull] Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (!type.IsEnum) throw new ArgumentException("Тип не является перечислением", nameof(type));
            Type = type;
        }

        public override object ProvideValue(IServiceProvider sp)
        {
            var values = _Type
               .GetFields()
               .Where(field => field.FieldType == _Type)
               .ToDictionary(
                    field => field.Name,
                    field => field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? field.Name);
            switch (NullValue)
            {
                case NullValueLocation.None:
                    return values.ToArray();
                case NullValueLocation.First:
                    return values.AppendFirst(new KeyValuePair<string, string>(null, string.Empty)).ToArray();
                case NullValueLocation.Last:
                    return values.AppendLast(new KeyValuePair<string, string>(null, string.Empty)).ToArray();
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}