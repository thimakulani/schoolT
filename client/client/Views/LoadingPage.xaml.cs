using client.Models;
using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var e = Preferences.Get("Email", null);
            var p = Preferences.Get("Password", null);
            if(e != null && p != null)
            {
                try
                {
                    UserLogin userLogin = new UserLogin()
                    {
                        Password = p,
                        Username = e
                    };

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(userLogin);
                    HttpContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient httpClient = new HttpClient();

                    var results = await httpClient.PostAsync("https://school-transport--api.herokuapp.com/api/auth/login", data);
                    if (results.IsSuccessStatusCode)
                    {
                        string str_out = await results.Content.ReadAsStringAsync();
                        AuthResponse authRespnse = new AuthResponse();
                        authRespnse = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthResponse>(str_out);
                        Preferences.Set("token", authRespnse.Token);
                        Preferences.Set("id", authRespnse.ApplicationUser.Id);
                        App.Current.MainPage = new AppShell();
                    }
                    else
                    {
                        App.Current.MainPage = new LoginPage();
                    }
                }
                catch (HttpRequestException)
                {
                    App.Current.MainPage = new LoginPage();
                }
            }
            
            else
            {
                App.Current.MainPage = new LoginPage();
            }
        }
    }
    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

