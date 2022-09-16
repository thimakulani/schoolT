using client.ViewModels;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Reactive;
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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
            CrossCloudFirestore.Current
                   .Instance
                   .Collection("USERS")
                   .ObserveAdded()
                   .Subscribe(document =>
                   {
                       Console.WriteLine(document.Document.Id + "weeeeeeeeeeee");
                   });
        }
    }
}