using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using ImageCircle.Forms.Plugin.Droid;
using System.Threading.Tasks;
using TodoList.Services.ImagePicker;

namespace TodoList.Droid
{
    [Activity(Label = "TodoList", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity This { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            ImageCircleRenderer.Init();
            LoadApplication(new App());
            This = this;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public int PickImageId { get; } = 1000;
        public TaskCompletionSource<ImagePickerData> PickImageTaskCompletionSource { get; set; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            TryImagePickerService(requestCode, resultCode, data);
        }

        private void TryImagePickerService(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 1000)
            {
                if ((resultCode == Result.Ok) && (data != null))
                {
                    Uri uri = data.Data;
                    PickImageTaskCompletionSource.SetResult(new ImagePickerData()
                    {
                        Stream = ContentResolver.OpenInputStream(uri),
                        FileName = GetFileName(uri),
                        //FilePath = Android.Net.Uri.Parse(uri.Path)
                    });
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }

        private string GetFileName(Uri uri)
        {
            var fileName = string.Empty;
            using (var query = ContentResolver.Query(uri, null, null, null))
            {
                var nameIndex = query.GetColumnIndex(OpenableColumns.DisplayName);
                query.MoveToFirst();
                fileName = query.GetString(nameIndex);
            }

            return fileName;
        }
    }
}