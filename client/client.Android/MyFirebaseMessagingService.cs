﻿
using Android.App;
using Android.Content;
using Android.Media;
using AndroidX.Core.App;
using client.Droid;
using Firebase.Messaging;

namespace client
{
    [Service]
    
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage remoteMessage)
        {
            if (remoteMessage.GetNotification() != null)
            {
                SendNotification(remoteMessage.GetNotification().Body, remoteMessage);
            }
            base.OnMessageReceived(remoteMessage);
        }


        void SendNotification(string messageBody, RemoteMessage remoteMessage)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0 /* Request code */, intent, PendingIntentFlags.OneShot);

            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this, "")
                .SetSmallIcon(Resource.Mipmap.icon)
                .SetContentTitle(remoteMessage.GetNotification().Title)
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetSound(defaultSoundUri)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);

            notificationManager.Notify(0 /* ID of notification */, notificationBuilder.Build());
        }
    }
}