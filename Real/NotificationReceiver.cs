using Android;
using Android.Content;

namespace Real;

[BroadcastReceiver(Enabled = true, Exported = true)]
public class NotificationReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        CreateNotification(context);
    }

    private void CreateNotification(Context context)
    {
        var notificationManager = NotificationManagerCompat.From(context);

        var notification = new NotificationCompat.Builder(context, "default")
            .SetContentTitle("Take a Photo")
            .SetContentText("It's time to take a photo!")
            //.SetSmallIcon(Resource.Drawable.ic_notification)
            .SetPriority(NotificationCompat.PriorityHigh)
            .SetAutoCancel(true)
            .Build();

        notificationManager.Notify(1, notification);
    }
}