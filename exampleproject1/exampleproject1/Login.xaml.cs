using exampleproject1.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;



//Deneme
namespace exampleproject1
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                SetValue(NavigationPage.HasNavigationBarProperty, false);
                InitializeComponent();
            }

        }
        public static int currentDeviceHeight = 0;
        protected  override void OnAppearing()
        {
            base.OnAppearing();
         
            currentDeviceHeight = (int)this.Height;


           

        }
       
        private  void LoginCheck(object sender, EventArgs e)
        {
        
        
            OnAppearing();
          

        }

     
    }
}
