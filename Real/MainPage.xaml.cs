using Android.Content;
using Android.OS;
using File = System.IO.File;
using Android;


namespace Real
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

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

        private void ImageButton_OnClicked(object? sender, EventArgs e)
        {
            TakePhoto();
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
