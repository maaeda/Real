using Plugin.LocalNotification;

namespace Real
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState activationState)//オーバーライドしているので、そのメソッドを呼び出す。
        {
            Window window= base.CreateWindow(activationState);//Windowクラスのインスタンスを作成する
            window.Stopped += (s, e) =>   //←window.ライフサイクルイベント名(今回はStopped)でイベントハンドラーを作成
            {
                //通知の処理
                var request = new NotificationRequest //ライブラリのNotificationRequestクラスを作成
                {
                    NotificationId = 1337,//通知のID(なんでもよい)
                    Title = "\u26a0\ufe0fRealの時間です。\u26a0\ufe0f",//(通知のタイトル)
                    //Subtitle = "撮影の通知",//(通知のサブタイトル)
                    Description = "今すぐRealを撮って友達にシェアしよう！",//(説明など)
                    BadgeNumber = 0,//(通知バッジのナンバー)
                    //通知のスケジュールを設定する
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5),//イベントが起こった5秒あとに通知をする処理
                    }
                };
                LocalNotificationCenter.Current.Show(request);//通知センターに通知を表示するメソッド
            };
            return window;//windowを返すようにする
        }
    }
}
