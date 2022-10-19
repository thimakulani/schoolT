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
