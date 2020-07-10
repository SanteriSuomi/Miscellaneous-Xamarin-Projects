using Xamarin.Forms;

namespace PhotoGallery.Data
{
    public class PhotoData
    {
        public static int TotalAmount { get; set; }

        public ImageSource Source { get; set; }
        public string Text { get; set; }
        public int Index { get; set; }
    }
}