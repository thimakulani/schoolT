using client.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace client.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private User user;
        public User User { get { return user;  } set { user = value; PropertyChanged(this, new PropertyChangedEventArgs("User")); }  }
        private string avatarText;
        public string AvatarText { get { return avatarText;  } set { avatarText = value; PropertyChanged(this, new PropertyChangedEventArgs("AvatarText")); }  }
        public ICommand OnAvatarClicked { get; set; }
        public ProfileViewModel()
        {
            OnAvatarClicked = new Command(ImagePicker);
            FetchUserInfo();
        }

        private void FetchUserInfo()
        {
           
        }

        private async void ImagePicker(object obj)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

            });


            if (file != null)
            {
                
            }
        }
    }
}
