using PhotoGallery.Data;
using PhotoGallery.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PhotoGallery
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private List<PhotoData> photoData;
        private int browseIndex;

        public MainPage()
        {
            InitializeComponent();
            Initialize();
        }

        private async void Initialize()
        {
            await CheckAndRequestStorageRead().ConfigureAwait(false);
            await CheckAndRequestStorageWrite().ConfigureAwait(false);
            GetImages();
        }

        private async Task<PermissionStatus> CheckAndRequestStorageRead()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>().ConfigureAwait(false);
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>().ConfigureAwait(false);
            }

            return status;
        }

        private async Task<PermissionStatus> CheckAndRequestStorageWrite()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>().ConfigureAwait(false);
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>().ConfigureAwait(false);
            }

            return status;
        }

        private void GetImages()
        {
            var fileService = DependencyService.Get<IGetPhotoDataService>();
            photoData = fileService.GetImages();
            ReloadImage();
        }

        private void OnBrowseLeft(object sender, EventArgs e)
        {
            if (browseIndex > 0)
            {
                browseIndex--;
            }

            ReloadImage();
        }

        private void OnBrowseRight(object sender, EventArgs e)
        {
            if (browseIndex < photoData.Count - 1)
            {
                browseIndex++;
            }

            ReloadImage();
        }

        private void ReloadImage()
        {
            mainImage.Source = photoData[browseIndex].Source;
            mainImageLabel.Text = photoData[browseIndex].Text;
            mainImageIndex.Text = $"{photoData[browseIndex].Index + 1}/{PhotoData.TotalAmount}";
        }
    }
}