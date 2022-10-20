using client.Models;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Essentials;
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

        private async void FetchUserInfo()
        {
            var id = Preferences.Get("id", null);
            HttpClient httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync($"{App.API_URL}/auth/user/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var str_resp = await response.Content.ReadAsStringAsync();
                    User = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(str_resp);
                }
            }
            catch (HttpRequestException EX)
            {
                var messageOptions = new MessageOptions
                {
                    Message = EX.Message,
                    Foreground = Color.Black
                };
                var options = new ToastOptions
                {
                    MessageOptions = messageOptions,
                    Duration = TimeSpan.FromMilliseconds(5000),
                    CornerRadius = new Thickness(10, 20, 30, 40),
                    BackgroundColor = Color.LightBlue
                };
                await App.Current.MainPage.DisplayToastAsync(options);
            }
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
