using SimpleTodoListMVVM.Utilities;
using System;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

namespace SimpleTodoListMVVM.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public Color TrueColor { get; set; }
        public Color FalseColor { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ConvertParametersToColor(parameter);
            return (value != null && value.Equals(true)) ? TrueColor : FalseColor;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S112:General exceptions should never be thrown", Justification = "<Pending>")]
        private void ConvertParametersToColor(object parameter)
        {
            if (parameter is null)
            {
                throw new NullReferenceException("Parameter shouldn't be null here,");
            }

            var parameterString = parameter as string;
            var parameters = parameterString.Split('|');
            var colors = new Color[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                colors[i] = ReflectionUtils.GetObjectFieldValue<Color>(parameters[i], BindingFlags.Public | BindingFlags.Static);
            }

            TrueColor = colors[0];
            FalseColor = colors[1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && ((Color)value).Equals(TrueColor);
        }
    }
}