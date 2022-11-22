using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using exampleproject1.Helpers;
using exampleproject1.Models;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace exampleproject1
{
    public partial class SignoutAlert 
    {
        public SignoutAlert()
        {
            InitializeComponent();
        }

        void Signout(System.Object sender, System.EventArgs e)
        {
            
                StaticClass.Instance.TotalPrice = 0;
                StaticClass.AllProducts = new System.Collections.ObjectModel.ObservableCollection<ProductListModel>();
                StaticClass.productId = 0;
                Settings.AccessToken = "";
                PopupNavigation.Instance.PopAsync(true);

            Application.Current.MainPage = new NavigationPage(new Login());

            



        }

        void CancelSignout(System.Object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);

        }

        async Task destroyPopUpAsync()
        {
            await PopupNavigation.Instance.PopAsync(true);

        }
    }
}
