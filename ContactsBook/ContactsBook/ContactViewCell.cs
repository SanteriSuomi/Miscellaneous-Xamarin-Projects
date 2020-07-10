using Xamarin.Forms;

namespace ContactsBook.Cells
{
    public class ContactViewCell : ViewCell
    {
        public static readonly BindableProperty SelectedItemBackgroundColorProperty
            = BindableProperty.Create("SelectedItemBackgroundColor",
                                      typeof(Color),
                                      typeof(ContactViewCell),
                                      Color.Default);

        public Color SelectedItemBackgroundColor
        {
            get => (Color)GetValue(SelectedItemBackgroundColorProperty);
            set => SetValue(SelectedItemBackgroundColorProperty, value);
        }
    }
}