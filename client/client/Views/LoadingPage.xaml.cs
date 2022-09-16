using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
      
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            CrossFirebaseAuth.Current.Instance.AuthState += (sender, e) =>
            {
                if (e.Auth.CurrentUser != null)
                {
                    App.Current.MainPage = new AppShell();
                }
                else
                {
                    App.Current.MainPage = new LoginPage();
                }
            };
        }
    }
}