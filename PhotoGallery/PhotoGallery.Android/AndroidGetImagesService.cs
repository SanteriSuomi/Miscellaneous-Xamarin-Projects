using Android.OS;
using PhotoGallery.Data;
using PhotoGallery.Services;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoGallery.Droid.Services.AndroidGetImagesService))]
namespace PhotoGallery.Droid.Services
{
    public class AndroidGetImagesService : IGetPhotoDataService
    {
        public List<PhotoData> GetImages()
        {
            string path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDcim).Path;
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            List<PhotoData> imageDatas = new List<PhotoData>();
            for (int i = 0; i < files.Length; i++)
            {
                var data = new PhotoData
                {
                    Source = ImageSource.FromFile(files[i]),
                    Text = Path.GetFileName(files[i]),
                    Index = i
                };

                imageDatas.Add(data);
            }

            PhotoData.TotalAmount = files.Length + 1;
            return imageDatas;
        }
    }
}