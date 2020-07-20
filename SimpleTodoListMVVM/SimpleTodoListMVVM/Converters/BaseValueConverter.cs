using System;
using System.Globalization;
using Xamarin.Forms;

namespace SimpleTodoListMVVM.Converters
{
    public class BaseValueConverter<T1, T2> : IValueConverter
    {
        public T1 TrueObject { get; set; }
        public T1 FalseObject { get; set; }

        public T2 ConvertCondition { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(ConvertCondition) ? TrueObject : FalseObject;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((T1)value).Equals(TrueObject);
        }
    }
}
