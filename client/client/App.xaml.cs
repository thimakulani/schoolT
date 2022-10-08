using BackendlessAPI;
using client.Views;
using Plugin.FirebaseAuth;
using Plugin.FirebasePushNotification;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace client
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new LoadingPage();
            Backendless.InitApp("A057CF45-40EA-B307-FFD8-3B93B6C90700", "223371CF-04E8-43AC-851C-0F924D583C0C");
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
