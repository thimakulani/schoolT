using BackendlessAPI;
using BackendlessAPI.Exception;
using client.Views;
using Google.Apis.Auth.OAuth2.Flows;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        //private readonly IGoogleClientManager _googleClientManager;
        private void ForgotPassword()
        {
            //var result = await DependencyService.Get<IGoogleLogin>().SignIn();
            //if (result.IsSuccess)
            //{
            //    //imgProfile.Source = result.Image;
            //    var name = result.Name;
            //    await App.Current.MainPage.DisplayAlert("", "Account Name -" + name, "Ok");
            //}
            //var response = await _googleClientManager.LoginAsync();
            //if(response != null)
            //{

            //}


            //_googleClientManager.OnLogin += async (s, e) =>
            //{
            //    if(e.Data != null)
            //    {
            //        GoogleUser googleUser = e.Data;
            //        var credential = CrossFirebaseAuth.Current.GoogleAuthProvider.GetCredential(_googleClientManager.IdToken, _googleClientManager.AccessToken);
            //        var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);
            //        if(result.User != null)
            //        {
            //            var query = CrossCloudFirestore.Current.Instance
            //                .Collection("USERS")
            //                .Document(result.User.Uid);
            //            var data = await query.GetAsync();
            //            if (!data.Exists)
            //            {
            //                Dictionary<string, object> pairs = new Dictionary<string, object>();
            //                pairs.Add("Name", googleUser.Name);
            //                pairs.Add("Surname", googleUser.FamilyName);
            //                pairs.Add("Email", googleUser.Email);
            //                pairs.Add("Url", googleUser.Picture.ToString());
            //                pairs.Add("Phone", null);
            //                await query.SetAsync(pairs);

            //            }
            //        }
            //        //CrossFirebaseAuth.Current.GoogleAuthProvider.
            //    };
            //};
            //_googleClientManager.OnError += async (s, e) =>
            //{
            //    await App.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
            //};
            //_googleClientManager.OnLogin += _googleClientManager_OnLogin;
            //try
            //{
            //    var results = await _googleClientManager.LoginAsync();
            //    Console.WriteLine(results.Message + "yeah");
            //}
            //catch (GoogleClientSignInNetworkErrorException e)
            //{
            //    await App.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
            //}
            //catch (GoogleClientSignInCanceledErrorException e)
            //{
            //    await App.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
            //}
            //catch (GoogleClientSignInInvalidAccountErrorException e)
            //{
            //    await App.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
            //}
            //catch (GoogleClientSignInInternalErrorException e)
            //{
            //    await App.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
            //}
            //catch (GoogleClientNotInitializedErrorException e)
            //{
            //    await App.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
            //}
            //catch (GoogleClientBaseException e)
            //{
            //    await App.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
            //}

        }

        //private void _googleClientManager_OnLogin(object sender, GoogleClientResultEventArgs<GoogleUser> e)
        //{
        //    App.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
        //}

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
                    Email = "thimakulani@gmail.com";
                    Password = "1234567890";
                    if (Email == null)
                    {
                        //await App.Current.MainPage.DisplayAlert("Error", "Enter email", "Ok");
                        EmailError = "Required";
                        return;
                    }
                    if (Password == null)
                    {
                        PasswordError = "Required";
                        //await App.Current.MainPage.DisplayAlert("Error", "Enter Password", "Ok");
                        return;
                    }
                    IsBusy = true;
                    var user = await Backendless.UserService.LoginAsync(Email, Password, true);
                    if(user != null)
                    {
                        App.Current.MainPage = new AppShell();
                    }
                }
                catch (BackendlessException ex)
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