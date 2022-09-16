using client.Models;
using Plugin.CloudFirestore;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace client.ViewModels
{
    public class SignUpViewModel: INotifyPropertyChanged
    {
        private string name;
        private string lastname;
        private string password;
        private string phoneNumber;
        private string email;
        private string role;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get { return name; } set { name = value; PropertyChanged(this, new PropertyChangedEventArgs("Name")); } }
        public string LastName { get { return lastname; } set { lastname = value; PropertyChanged(this, new PropertyChangedEventArgs("LastName")); } }
        public string Password { get { return password; } set { password = value; PropertyChanged(this, new PropertyChangedEventArgs("Password")); } }
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; PropertyChanged(this, new PropertyChangedEventArgs("PhoneNumber")); } }
        public string Email { get { return email; } set { email = value; PropertyChanged(this, new PropertyChangedEventArgs("Email")); } }
        public string Role { get { return role; } set { role = value; PropertyChanged(this, new PropertyChangedEventArgs("Role")); } }

        public ICommand OnSignUpCommand { get; set; }
        private async void Signup()
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
                    Email = Email,
                    PhoneNumber = PhoneNumber,
                    FirstName = Name,
                    LastName = LastName,
                    Role = Role,
                };
                var results = await Plugin.FirebaseAuth.CrossFirebaseAuth
                    .Current
                    .Instance
                    .CreateUserWithEmailAndPasswordAsync(Email, Password);

                user.Id = results.User.Uid;
                await CrossCloudFirestore
                    .Current
                    .Instance
                    .Collection("USERS")
                    .Document(results.User.Uid)
                    .SetAsync(user);
            }
            catch (Plugin.FirebaseAuth.FirebaseAuthException ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Okay");
            }
        }
        public SignUpViewModel() 
        {
            OnSignUpCommand = new Command(Signup);
        }

    }

}
