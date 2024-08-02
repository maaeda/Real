using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Content;
using Android.OS;
using File = System.IO.File;
using Android;


namespace Real
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadPhotos();
        }

        private void OnTakePhotoBtnClicked(object sender, EventArgs e)
        {
            TakePhoto();
        }

        private void OnLoadPhotosClicked(object? sender, EventArgs e)
        {
            LoadPhotos();
        }

        private async void ImageButton_OnClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.BindingContext is string imagePath)
            {
                await DisplayAlert("Image Selected", $"You selected {imagePath}", "OK");

                // 画像共有の実装
                await ShareImage(imagePath);
            }
        }

        private async Task ShareImage(string imagePath)
        {
            // 画像の共有処理をここに実装します
            // 以下はサンプルとしてファイルを共有するコードです

            // 実際の画像パスに置き換えてください
            var filePath = Path.Combine(FileSystem.AppDataDirectory, imagePath);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Share Image",
                File = new ShareFile(filePath)
            });
        }

        private async void TakePhoto()
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
    }
}
