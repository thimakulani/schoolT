using client.Views;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace client.Services
{
    public class SendPushNotification
    {
   
        private FirebaseMessaging GetFirebaseMessaging()
        {
            var assembly = typeof(HomePage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"client.JsonResources.account_service.json");
            string json = null;
            using (var stramerader = new System.IO.StreamReader(stream))
            {
                json = stramerader.ReadToEnd();
            }
            FirebaseMessaging messaging;

            FirebaseAdmin.AppOptions options = new FirebaseAdmin.AppOptions()
            {
                Credential = GoogleCredential.FromJson(json),
                ProjectId = "school-transport-fc879",
                ServiceAccountId = "firebase-adminsdk-56jex@school-transport-fc879.iam.gserviceaccount.com",

            };
            if (FirebaseAdmin.FirebaseApp.DefaultInstance == null)
            {
                var app = FirebaseAdmin.FirebaseApp.Create(options);
                messaging = FirebaseMessaging.GetMessaging(app);
            }
            else
            {
                messaging = FirebaseMessaging.DefaultInstance;
            }

            return messaging;
        }
        public async void SendMessage(string topic, string message, string title)
        {
            var fcm = GetFirebaseMessaging();
            Message msg = new Message()
            {
                Topic = topic,
                Notification = new Notification()
                {
                    Title = title,
                    Body = $"{message}",
                },
            };
            var results = await fcm.SendAsync(msg);
            Console.WriteLine("wwwwww" + results);
        }
    }
}
