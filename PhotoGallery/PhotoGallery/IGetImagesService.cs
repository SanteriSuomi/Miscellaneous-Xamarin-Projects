using PhotoGallery.Data;
using System.Collections.Generic;

namespace PhotoGallery.Services
{
    public interface IGetPhotoDataService
    {
        List<PhotoData> GetImages();
    }
}