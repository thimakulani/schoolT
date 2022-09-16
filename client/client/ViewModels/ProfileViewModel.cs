using client.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            CrossCloudFirestore
                .Current
                .Instance
                .Collection("USERS")
                .Document(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                .AddSnapshotListener((v, e) =>
                {
                    if (v.Exists)
                    {
                        User = v.ToObject<User>();
                    }
                });
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
                var storage_ref = Plugin.FirebaseStorage.CrossFirebaseStorage
                     .Current
                     .Instance
                     .RootReference
                     .Child(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid);

                await storage_ref.PutStreamAsync(file.GetStream());

                var url = await storage_ref.GetDownloadUrlAsync();

                //    .PutStreamAsync(file.GetStream());
                await CrossCloudFirestore
                    .Current
                    .Instance
                    .Collection("USERS")
                    .Document(CrossFirebaseAuth.Current.Instance.CurrentUser.Uid)
                    .UpdateAsync("Url", url.ToString());
            }
        }
    }
}
