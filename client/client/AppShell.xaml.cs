using client.ViewModels;
using client.Views;
using Plugin.FirebaseAuth;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace client
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(RequstsPage), typeof(RequstsPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(LogoutPage), typeof(LogoutPage));

            CrossFirebasePushNotification.Current.Subscribe(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid);
        
        }

    }
}
