using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Real
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _photos;
        public ObservableCollection<string> Photos
        {
            get => _photos;
            set
            {
                _photos = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            // 初期データのロード
            Photos = new ObservableCollection<string>
            {
                "image1.png",
                "image2.png",
                "image3.png"
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}