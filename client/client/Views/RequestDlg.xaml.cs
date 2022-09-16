using client.Models;
using client.Services;
using Plugin.CloudFirestore;
using Plugin.FirebaseAuth;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestDlg
    {
        public Request request;
        public RequestDlg(Models.Request request)
        {
            InitializeComponent();
            this.request = request;
            BindingContext = this.request;
        }

        private async void ImgClose_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void BntReq_Clicked(object sender, EventArgs e)
        {
            var d = await findNearestDriversAsync();
            //Console.WriteLine(d.Id);
            if (!string.IsNullOrEmpty(d.Id))
            {
                request.UserId = CrossFirebaseAuth.Current.Instance.CurrentUser.Uid;
                request.Status = "Pending";
                request.RequestTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                var query = await CrossCloudFirestore
                                      .Current.Instance
                                      .Collection("REQUESTS")
                                      .AddAsync(request);

                if (query != null)
                {

                    Dictionary<string, object> Data = new Dictionary<string, object>();
                    Data.Add("R_ID", query.Id);
                    //Data.Add("D_REF", query);

                    await CrossCloudFirestore
                        .Current
                        .Instance
                        .Collection("UPCOMING")
                        .Document(d.Id)
                        .SetAsync(Data);
                    SendPushNotification sendPush = new SendPushNotification();
                    sendPush.SendMessage(d.Id, "You have a new request", "New Request");
                }

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Driver", "The are no active drivers at a moment", "OK");
            }




        }
       private async Task<Driver> findNearestDriversAsync()
        {
            Driver driver = new Driver();
            var q = await CrossCloudFirestore
                .Current
                .Instance
                .Collection("USERS")
                .WhereEqualsTo("Status", "Online")
                .WhereEqualsTo("Role", "User")
                .GetAsync();
            var position = request.PickupLat;
            if(!q.IsEmpty)
            {
                var drivers = q.ToObjects<Driver>().ToList();
                if (drivers.Count > 1)
                {
                    double heighest = UnitConverters.CoordinatesToKilometers(request.PickupLat, request.PickupLong, drivers[0].Latitude, drivers[0].Longitude);
                    for (int i = 0; i < drivers.Count; i++)
                    {
                        var distance = UnitConverters.CoordinatesToKilometers(request.PickupLat, request.PickupLong, drivers[i].Latitude, drivers[i].Longitude);

                        if (heighest < distance)
                        {
                            driver = drivers[i];
                        }
                    }
                }
                else
                {
                    driver = drivers[0];
                }
            }
            return driver;
        } 
    }
}