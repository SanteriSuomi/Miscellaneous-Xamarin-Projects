using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhotoGallery.Extensions.Markups
{
    internal class EmbeddedResourceImage : IMarkupExtension
    {
        public string ResourceID { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(ResourceID))
            {
                System.Diagnostics.Debug.Write("ResouceID is empty.");
                return null;
            }

            return ImageSource.FromResource(ResourceID);
        }
    }
}
