using SimpleTodoListMVVM.Converters;
using Xunit;
using Xamarin.Forms;

namespace SimpleTodoListMVVM.Tests
{
    public class BoolToColorConverterTests
    {
        [Fact]
        public void Convert_returns_type_color_when_completed()
        {
            var converter = new BoolToColorConverter();
            var result = converter.Convert(true, null, "Black|Blue", null);
            Assert.IsType<Color>(result);
        }

        [Fact]
        public void Convert_returns_correct_second_parameter_when_false_and_completed()
        {
            var converter = new BoolToColorConverter();
            var result = (Color)converter.Convert(false, null, "Black|Blue", null);
            Assert.Equal(Color.Blue, result);
        }
    }
}