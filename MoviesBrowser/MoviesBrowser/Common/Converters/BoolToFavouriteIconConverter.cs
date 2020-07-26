using System;
using System.Globalization;
using Xamarin.Forms;

namespace MoviesBrowser.Common.Converters
{
    public class BoolToFavouriteIconConverter : IValueConverter
    {
        private const string favouriteIconOn = "favorite_on.png";
        private const string favouriteIconOff = "favorite_off.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return favouriteIconOn;
            }

            return favouriteIconOff;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}