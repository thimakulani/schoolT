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
using BackendlessAPI;
using BackendlessAPI.Exception;
using System.Reflection;
using System.Linq;

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
        private void Signup()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
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
                    User user = new User()
                    {
                        PhoneNumber = PhoneNumber,
                        FirstName = Name,
                        LastName = LastName,
                        Role = "User",
                        ImageUrl = null,
                       
                    };
                    var dick = user.GetType()
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .ToDictionary(prop => prop.Name.ToLower(), prop => prop.GetValue(user, null));

                    BackendlessUser _backendlessUser = new BackendlessUser()
                    {
                        Properties = dick,
                        Email = Email,
                        Password = Password,
                    };
                    
                    IsBusy = true;
                    var results = await Backendless.UserService.RegisterAsync(_backendlessUser);
                }
                catch (BackendlessException ex)
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
        }

    }

}
