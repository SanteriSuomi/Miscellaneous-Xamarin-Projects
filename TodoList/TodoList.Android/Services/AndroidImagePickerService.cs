using Android.Content;
using System.Threading.Tasks;
using TodoList.Droid.Services;
using TodoList.Services.ImagePicker;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidImagePickerService))]
namespace TodoList.Droid.Services
{
    public class AndroidImagePickerService : IImagePickerService
    {
        public Task<ImagePickerData> GetImageDataAsync()
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            MainActivity.This.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"),
                                                     MainActivity.This.PickImageId);

            MainActivity.This.PickImageTaskCompletionSource = new TaskCompletionSource<ImagePickerData>();
            return MainActivity.This.PickImageTaskCompletionSource.Task;
        }
    }
}