using SimpleTodoListMVVM.Utilities;
using Xamarin.Forms;
using Xunit;

namespace SimpleTodoListMVVM.Tests
{
    public class ReflectionUtilsTests
    {
        [Fact]
        public void GetObjectFieldValue_returns_correct_field_value()
        {
            var fieldName = "Blue";
            var fieldValue = ReflectionUtils.GetObjectFieldValue<Color>(fieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            Assert.Equal(Color.Blue, fieldValue);
        }
    }
}