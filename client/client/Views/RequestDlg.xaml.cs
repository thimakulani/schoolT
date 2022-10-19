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
            var d = await FindNearestDriversAsync();
            //Console.WriteLine(d.Id);
            if (!string.IsNullOrEmpty(d))
            {
                request.UserId = "";
                request.Status = "Pending";
                request.DriverId = d;
                request.RequestTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                //await Backendless.Data.Of<Request>().SaveAsync(request);

                //    await CrossCloudFirestore
                //        .Current
                //        .Instance
                //        .Collection("UPCOMING")
                //        .Document(d)
                //        .SetAsync(Data);
                //    SendPushNotification sendPush = new SendPushNotification();
                //    sendPush.SendMessage(d, "You have a new request", "New Request");
                //}

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Driver", "The are no active drivers at a moment", "OK");
            }




        }
        private DriverLocation d_l = new DriverLocation();
       private async Task<string> FindNearestDriversAsync()
       {
            //var all_users = await Backendless.Data.Of<BackendlessUser>().FindAsync();
            //var users = all_users.Where(x => (string)x.GetProperty("role") == "Driver").ToList();

            //var d_ = users.Select(x=>x.ObjectId).ToArray();

            //var query = await CrossCloudFirestore
            //    .Current
            //    .Instance
            //    .Collection("LOCATION")
            //    .WhereIn(FieldPath.DocumentId, d_)
            //    .GetAsync();
            //string id = null;
            //if(!query.IsEmpty)
            //{
            //    var Loc = query.ToObjects<DriverLocation>().ToList();
            //    double heighest = UnitConverters
            //        .CoordinatesToKilometers(request.PickupLat, request.PickupLong, Loc[0].Latitude, Loc[0].Longitude);
            //    for (int i = 0; i < Loc.Count; i++)
            //    {
            //        var distance = UnitConverters.CoordinatesToKilometers(request.PickupLat, request.PickupLong, Loc[i].Latitude, Loc[i].Longitude);

            //        if (heighest < distance)
            //        {
            //            id = Loc[i].Uid;
            //        }
            //    }
            //}
            return "";
        } 
    }
}