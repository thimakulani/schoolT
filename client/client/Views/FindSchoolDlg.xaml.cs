using client.Models;
using Plugin.MaterialDesignControls;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindSchoolDlg
    {
        private ObservableCollection<Schools> schools = new ObservableCollection<Schools>();
        public ObservableCollection<Schools> Schools { get { return schools; } set { schools = value; } }
        public FindSchoolDlg()
        {
            InitializeComponent();
           
            SchooItems.ItemsSource = Schools;
            FetchSchools();
        }
        public void FetchSchools()
        {
            Device.InvokeOnMainThreadAsync(() =>
            {
                
           });

        }

        private async void ImgClose_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
        public event EventHandler<SelectedSchool> SelectedSchoolHandler;
        public class SelectedSchool : EventArgs
        {
            public Schools School { get; set; }
        }
        private async void SchooItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = e.CurrentSelection.FirstOrDefault() as Schools;
            await DisplayAlert("", selected.Id, "Cool");
            SelectedSchoolHandler.Invoke(this, new SelectedSchool { School = selected });
            await PopupNavigation.Instance.PopAsync();
        }
    }
}