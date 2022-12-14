using client.Models;
using OSRMLib.OSRMServices;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Reactive;
using Plugin.FirebaseAuth;
using Plugin.FirebasePushNotification;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public  HomePage()
        {
            InitializeComponent();
            BindingContext = this;
            var assembly = typeof(HomePage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"client.JsonResources.MapTheme.json");
            string themeFile;
            using (var reader = new System.IO.StreamReader(stream))
            {
                themeFile = reader.ReadToEnd();
                G_map.MapStyle = MapStyle.FromJson(themeFile);
            }
            
            InitMap();
            GetDrivers();
            LoadSchoolsOnMap();
            //CrossFirebaseAuth.Current.Instance.SignOut();
            Test();
        }

        private void Test()
        {
            
        }

    

        private ObservableCollection<Driver> driverLocations = new ObservableCollection<Driver>();
        public ObservableCollection<Driver> DriverLocations { get { return driverLocations; } }        
        private ObservableCollection<Schools> schools = new ObservableCollection<Schools>();
        public ObservableCollection<Schools> Schools { get { return schools; } }
        private void GetDrivers()
        {
            
        }

        private void removeMap()
        {
            
        }

        private void addMapPin(Driver dl)
        {
            Pin pin = new Pin()
            {
                Label = dl.Name,
                Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("CarPins.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "CarPins.png", WidthRequest = 30, HeightRequest = 30 }),
                Position = new Position(dl.Latitude, dl.Longitude),
            };
            G_map.Pins.Add(pin);
        }

        private async void InitMap()
        {
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            G_map.CameraIdled += G_map_CameraIdled;
            var pos = await Geolocation.GetLocationAsync();
            if (pos != null)
            {
                G_map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(pos.Latitude, pos.Longitude), Distance.FromMeters(500)));
            }
        }
        private Location P_Location = new Location();
        private Location D_Location = new Location();
        bool flag = true;
        private async void G_map_CameraIdled(object sender, CameraIdledEventArgs e)
        {
            var pos = new Location(e.Position.Target.Latitude, e.Position.Target.Longitude);
            try
            {
                if (RdbPick.IsChecked && flag)
                {
                    LblPickup.Text = $"{await GetAddress(pos)}";
                    P_Location = pos;
                }
                
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task<string> GetAddress(Location latlang)
        {
            Geocoder geocode = new Geocoder();
            var address = await geocode.GetAddressesForPositionAsync(new Position(latlang.Latitude, latlang.Longitude));
            return address?.FirstOrDefault().ToString();
        }

        private void LoadSchoolsOnMap()
        {
            Device.InvokeOnMainThreadAsync(() =>
            {
                
            });
        }
        private void addMap(Schools dl)
        {
            Pin pin = new Pin()
            {
                Label = dl.Name,
                Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("school_square_icon.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "CarPins.png", WidthRequest = 30, HeightRequest = 30 }),
                Position = new Position(dl.Latitude, dl.Longitude),
            };
            G_map.Pins.Add(pin);
        }
        private async void BtnContinue_Clicked(object sender, EventArgs e)
        {
            if(BtnContinue.Text == "Continue".ToUpper())
            {
                if (P_Location != null && D_Location != null)
                {
                    //send request
                    var startLocation = new OSRMLib.Helpers.Location(P_Location.Latitude, P_Location.Longitude);
                    var destLocation = new OSRMLib.Helpers.Location(D_Location.Latitude, D_Location.Longitude);
                    GetRoute(startLocation, destLocation);
                    flag = false;
                    
                }
            }
            else if (BtnContinue.Text == "Request".ToUpper())
            {
                await PopupNavigation.Instance.PushAsync(new RequestDlg(request));
            }
            

        }

        readonly RouteService routeS = new RouteService();
        Request request = new Request();
        public async void GetRoute(OSRMLib.Helpers.Location startPos, OSRMLib.Helpers.Location endPos)
        {

            routeS.Coordinates = new List<OSRMLib.Helpers.Location> { startPos, endPos };
            BtnContinue.IsBusy = true;
            var response = await routeS.Call();

            Polyline polyline = new Polyline()
            {
                StrokeColor = Color.CadetBlue,
                StrokeWidth = 12,

            };
            Pin p_pin = new Pin()
            {
                Position = new Position(startPos.Latitude, startPos.Longitude),
                Address = LblPickup.Text,
                Label = "Pickup"


            };
            Pin d_pin = new Pin()
            {
                Position = new Position(endPos.Latitude, endPos.Longitude),
                Address = LblDest.Text,
                Label = "Destination"
            };
            var points = response.Routes[0].Geometry;
            foreach (var point in points)
            {
                polyline.Positions.Add(new Position(point.Latitude, point.Longitude));
            }

            G_map.Pins.Add(p_pin);
            G_map.Pins.Add(d_pin);
            G_map.Polylines.Add(polyline);

            double distance = Math.Round(response.Routes[0].Legs[0].Distance / 1000, 2);
            G_map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(startPos.Latitude,startPos.Longitude), Distance.FromMeters(500)));
            request.Distance = distance;
            request.PickupAddress = LblPickup.Text;
            request.DestinationAddress = LblDest.Text;
            double rate = 20;
            double afterInitial = 10;
           

            double fares;
            if (distance <= 2)
            {
                fares = rate;
            }
            else
            {
                fares = ((distance - 2) * afterInitial) + rate;

            }
            request.Price = Math.Round(fares);

            request.PickupLat = startPos.Latitude;
            request.PickupLong = startPos.Longitude;

            request.DestinationLat = endPos.Latitude; 
            request.DestinationLong = endPos.Longitude;

            request.EstimatedTime = $"{Math.Round(response.Routes[0].Duration / 60)} Minutes";
            BtnContinue.IsBusy = false;
            BtnContinue.Text = "Request".ToUpper();
        }

        private async void RdbDest_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if(e.Value)
            {
                var school = new FindSchoolDlg();
                school.SelectedSchoolHandler += School_SelectedSchoolHandler;
                await PopupNavigation.Instance.PushAsync(school, true);
                //LblDest.Text = $"{await GetAddress(pos)}";
                //D_Location = pos;
            }
        }

        private void School_SelectedSchoolHandler(object sender, FindSchoolDlg.SelectedSchool e)
        {
            LblDest.Text = e.School.Name;
            D_Location.Latitude = e.School.Latitude;
            D_Location.Longitude = e.School.Longitude;
        }
    }
}