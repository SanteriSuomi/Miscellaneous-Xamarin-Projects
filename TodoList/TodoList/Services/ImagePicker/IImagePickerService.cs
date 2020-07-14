using System.Threading.Tasks;

namespace TodoList.Services.ImagePicker
{
    public interface IImagePickerService
    {
        Task<ImagePickerData> GetImageDataAsync();
    }
}