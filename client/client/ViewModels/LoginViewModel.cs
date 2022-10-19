using client.Models;
using client.Views;
using Google.Apis.Auth.OAuth2.Flows;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace client.ViewModels
{
    public class LoginViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string email;
        private string password;
        private bool isBusy;  
        private string emailError;  
        private string passwordError;   
        public string EmailError { get { return emailError; } set { emailError = value; PropertyChanged(this, new PropertyChangedEventArgs("EmailError")); } }
        public string PasswordError { get { return passwordError; } set { passwordError = value; PropertyChanged(this, new PropertyChangedEventArgs("PasswordError")); } }
        public bool IsBusy { get { return isBusy; } set { isBusy = value; PropertyChanged(this, new PropertyChangedEventArgs("IsBusy")); } }

        public string Email { get { return email; } set { email = value; PropertyChanged(this, new PropertyChangedEventArgs("Email")); } }
        public string Password { get { return password; } set { password = value; PropertyChanged(this, new PropertyChangedEventArgs("Password")); } }

        public ICommand SubmitCommand { get; set; }
        public ICommand OnSignUpNavigation { get; set; }
        public ICommand OnForgotPassword { get; set; }
        public LoginViewModel()
        {
            SubmitCommand = new Command(Login);
            OnSignUpNavigation = new Command(SignUp);
            OnForgotPassword = new Command(ForgotPassword);
            //_googleClientManager = CrossGoogleClient.Current;
            
        }
        private void ForgotPassword()
        {

        }
        private void SignUp()
        {
            App.Current.MainPage.Navigation.PushModalAsync(new SignupPage());
        }
        
        private void Login()
        {
            MainThread.BeginInvokeOnMainThread( async() =>
            {
                try
                {
                    Email = "thima@gmail.com";
                    Password = "THIma$!305";
                    if (Email == null)
                    {
                        EmailError = "Required";
                        return;
                    }
                    if (Password == null)
                    {
                        PasswordError = "Required";
                        return;
                    }
                    IsBusy = true;
                    UserLogin userLogin = new UserLogin()
                    {
                        Password = Password,
                        Username = Email,
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
                        Preferences.Set("Email", Email);
                        Preferences.Set("Password", Password);
                        App.Current.MainPage = new AppShell();
                    }
                    else
                    {
                        string str_out = await results.Content.ReadAsStringAsync();
                        await App.Current.MainPage.DisplayAlert("Error", str_out, "Got it");
                    }
                }
                catch (HttpRequestException ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                }
                finally
                {
                    IsBusy = false;
                }
            });

             



                
            
            

        }
    }
}