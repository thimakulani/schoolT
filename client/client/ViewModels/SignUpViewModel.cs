using client.Models;
using Plugin.CloudFirestore;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Reflection;
using System.Linq;
using System.Net.Http;
using client.Views;

namespace client.ViewModels
{
    public class SignUpViewModel: INotifyPropertyChanged
    {
        private string name;
        private string lastname;
        private string password;
        private string phoneNumber;
        private string email;
        private bool isBusy;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get { return name; } set { name = value; PropertyChanged(this, new PropertyChangedEventArgs("Name")); } }
        public string LastName { get { return lastname; } set { lastname = value; PropertyChanged(this, new PropertyChangedEventArgs("LastName")); } }
        public string Password { get { return password; } set { password = value; PropertyChanged(this, new PropertyChangedEventArgs("Password")); } }
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; PropertyChanged(this, new PropertyChangedEventArgs("PhoneNumber")); } }
        public string Email { get { return email; } set { email = value; PropertyChanged(this, new PropertyChangedEventArgs("Email")); } }
        public bool IsBusy { get { return isBusy; } set { isBusy = value; PropertyChanged(this, new PropertyChangedEventArgs("IsBusy")); } }

        public ICommand OnSignUpCommand { get; set; }
        public Command OnBackCommand { get; set; }

        private void Signup()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                Email = "thimakulani2@gmail.com";
                Password = "THIma$!305";

                if (Email == null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Enter email", "Got it");
                    return;
                }
                if (Password == null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Enter Password", "Got it");
                    return;
                }
                try
                {
                    IsBusy = true;
                    User user = new User()
                    {
                        Phone = "1234567890",
                        FirstName = "Thima",
                        LastName = "Sigauque",
                        UserType = "User",
                        ImageUrl = "x",
                        Password = Password,
                        AccountStatus = "active",
                        Email = Email,
                        OnlineStatus = "active"
                        
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                    HttpContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient httpClient = new HttpClient();
                    var results = await httpClient.PostAsync($"https://school-transport--api.herokuapp.com/api/auth/register", data);
                    if (results.IsSuccessStatusCode)
                    {
                        var str = await results.Content.ReadAsStringAsync();
                        var response = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthResponse>(str);
                        if(response.Token != null)
                        {
                            await App.Current.MainPage.DisplayAlert("Success", "Your account has been successfully created", "Got it");
                            App.Current.MainPage = new LoginPage();
                        }
                    }
                    else
                    {
                        var str_r = await results.Content.ReadAsStringAsync();
                        await App.Current.MainPage.DisplayAlert("Failed", str_r, "Got It");
                    }
                }
                catch (HttpRequestException ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Okay");
                }
                finally
                {
                    IsBusy = false;
                }
            });     
        }
        public SignUpViewModel() 
        {
            OnSignUpCommand = new Command(Signup);
            OnBackCommand = new Command(Login);
        }

        private void Login(object obj)
        {
            App.Current.MainPage.Navigation.PopModalAsync();
        }
    }

}
