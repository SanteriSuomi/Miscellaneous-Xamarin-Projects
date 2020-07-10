using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using ContactsBook.Cells;
using ContactsBook.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(ContactViewCell), typeof(ContactViewCellRenderer))]
namespace ContactsBook.Droid.Renderers
{
    public class ContactViewCellRenderer : ViewCellRenderer
    {
        private View cellCore;
        private Drawable unselectedBackground;
        private bool selected;

        protected override View GetCellCore(Cell item, View convertView, ViewGroup parent, Context context)
        {
            cellCore = base.GetCellCore(item, convertView, parent, context);
            unselectedBackground = cellCore.Background;
            selected = false;
            return cellCore;
        }

        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                var contactViewCell = sender as ContactViewCell;
                cellCore.SetBackgroundColor(contactViewCell.SelectedItemBackgroundColor.ToAndroid());
                selected = !selected;
                if (!selected)
                {
                    cellCore.SetBackground(unselectedBackground);
                }
            }
        }
    }
}