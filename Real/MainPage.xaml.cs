using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Content;
using Android.OS;
using File = System.IO.File;
using Android;
using Camera.MAUI;
using Camera.MAUI.ZXing;
using CommunityToolkit.Maui.Views;
using System.Diagnostics;


namespace Real
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadPhotos();
            cameraView.CamerasLoaded += CameraView_CamerasLoaded;
        }

        private void CameraView_CamerasLoaded(object sender, EventArgs e)
        {
            if (cameraView.NumCamerasDetected > 0)
            {
                if (cameraView.NumMicrophonesDetected > 0)
                    cameraView.Microphone = cameraView.Microphones.First();
                    cameraView.Camera = cameraView.Cameras.Last();//フロントカメラ
                    Dispatcher.Dispatch(async () =>
                    {
                        while (await cameraView.StartCameraAsync() != CameraResult.Success)
                        {
                        }

                    });
            }
        }

        //MAUI.Camera
        private async void TakePhoto()
        {
            var stream = await cameraView.TakePhotoAsync();

            /*デバック用*/
            /*
            if (stream != null)
            {
                var result = ImageSource.FromStream(() => stream);
                snapPreview.Source = result;
            }
            */
            /*デバック用*/

            if (stream != null)
            {
                // パブリックピクチャーディレクトリの取得
                string picturesDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
                string realDirectory = Path.Combine(picturesDirectory, "Real");

                // Realディレクトリが存在しない場合は作成
                if (!Directory.Exists(realDirectory))
                {
                    Directory.CreateDirectory(realDirectory);
                }

                string currentDate = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                string localFilePath = Path.Combine(realDirectory, $"{currentDate}.jpg");

                using Stream sourceStream = stream;
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);

                await ShareImage(localFilePath);

                // 写真を撮った後、グリッドを更新
                LoadPhotos();
            }
        }

        private void OnTakePhotoBtnClicked(object sender, EventArgs e)
        {
            TakePhoto();
        }

        private void OnLoadPhotosClicked(object? sender, EventArgs e)
        {
            LoadPhotos();
        }

        private void OnTakePhotoOnMedidaPicker(object? sender, EventArgs e)
        {
            TakePhotoOnMedidaPicker();
        }

        private async void ImageButton_OnClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.BindingContext is string imagePath)
            {
                /*デバック用*/
                //await DisplayAlert("Image Selected", $"You selected {imagePath}", "OK");
                /*デバック用*/

                // 画像共有
                await ShareImage(imagePath);
            }
        }

        private async Task ShareImage(string imagePath)
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, imagePath);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Share Image",
                File = new ShareFile(filePath)
            });
        }

        //MediaPicker CapturePhotoAsync
        private async void TakePhotoOnMedidaPicker()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    // パブリックピクチャーディレクトリの取得
                    string picturesDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
                    string realDirectory = Path.Combine(picturesDirectory, "Real");

                    // Realディレクトリが存在しない場合は作成
                    if (!Directory.Exists(realDirectory))
                    {
                        Directory.CreateDirectory(realDirectory);
                    }

                    string localFilePath = Path.Combine(realDirectory, photo.FileName);

                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);

                    await ShareImage(localFilePath);

                    // 写真を撮った後、グリッドを更新
                    LoadPhotos();
                }
            }
        }

        private void LoadPhotos()
        {
            string picturesDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
            string realDirectory = Path.Combine(picturesDirectory, "Real");

            if (Directory.Exists(realDirectory))
            {
                var photos = Directory.GetFiles(realDirectory)
                    .Where(file => file.EndsWith(".jpg") || file.EndsWith(".png"))
                    .ToList();

                PhotosCollectionView.ItemsSource = photos;
            }
        }

        private void OnCameraStartBtnClicked(object? sender, EventArgs e)
        {
            if (cameraView.NumCamerasDetected > 0)
            {
                if (cameraView.NumMicrophonesDetected > 0)
                    cameraView.Microphone = cameraView.Microphones.First();
                cameraView.Camera = cameraView.Cameras.Last();//フロントカメラ
                Dispatcher.Dispatch(async () =>
                {
                    while (await cameraView.StartCameraAsync() != CameraResult.Success)
                    {
                    }

                });
            }
        }
    }
}
