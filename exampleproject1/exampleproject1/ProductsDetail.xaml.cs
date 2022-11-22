using exampleproject1.Models;
using exampleproject1.Services;
using exampleproject1.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace exampleproject1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsDetail : ContentPage
    {
        public ProductsDetail()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
               App.Current.MainPage= new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                SetValue(NavigationPage.HasNavigationBarProperty, false);

                InitializeComponent();
            }


        }
        int currentDeviceHeight = 0;
      

       

        protected async override void OnAppearing()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                ProductViewModel.DetailProduct = await ProductServices.ProductById(ProductViewModel.DetailProduct.Id);
                description.Text = ProductViewModel.DetailProduct.Description;
                base.OnAppearing();
                currentDeviceHeight = (int)this.Height;

               
            }
        }




        private async void CloseProductDetail(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }


        bool isBackdropTapEnabled = true;

      

        private async void ClickedAddBasket(object sender, EventArgs e)
        {
            
            _ = DisplayAlert("Başarılı", "Ürün Sepete Eklendi", "OK");
           
                await CloseDrawer();

            
        }
        async void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {

            if (e.StatusType == GestureStatus.Running)
            {
                isBackdropTapEnabled = false;
              
                Debug.WriteLine($"Running: {e.TotalY}");
                if (e.TotalY > 0)
                {
                    BottomToolbar.TranslationY += e.TotalY;
                    Debug.WriteLine($"Running: {BottomToolbar.TranslationY}");

                }
                else
                {
                    if (BottomToolbar.TranslationY > currentDeviceHeight - 280)
                    {
                        BottomToolbar.TranslationY += e.TotalY;

                    }

                }

            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                if (BottomToolbar.TranslationY > (currentDeviceHeight - 280) + 130)
                {
                    await CloseDrawer();
                }
                else
                {
                    await OpenDrawer();
                }
                isBackdropTapEnabled = true;
            }
        }
        //popup açılış

        async Task CloseDrawer()
        {

            bglayout.IsVisible = false;

            await Task.WhenAll
            (

                BottomToolbar.TranslateTo(0, currentDeviceHeight, length: 250, easing: Easing.SinIn)
            );
            // ListProducts.ItemsSource = null;

        }
        //popup kapanış
        async Task OpenDrawer()
        {


            OnAppearing();
            bglayout.IsVisible = true;


            await Task.WhenAll
            (
                BottomToolbar.TranslateTo(0, currentDeviceHeight - 280, length: 250, easing: Easing.SinIn)
            );
        }

        async private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            if (isBackdropTapEnabled)
            {
                await CloseDrawer();
            }
        }
        async void CheckProductPopUp(System.Object sender, System.EventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                OnAppearing();
                Frame p = (Frame)sender;
                var tapGesture = (TapGestureRecognizer)p.GestureRecognizers[0];
                var item = (ProductListModel)tapGesture.CommandParameter;
                if (item.Variants.Count > 0)
                {

                    if (bglayout.IsVisible == true)
                    {
                        await CloseDrawer();
                    }
                    else
                    {
                        await OpenDrawer();
                    }

                }
                else
                {
                    _ = DisplayAlert("Başarılı", "Ürün Sepete Eklendi", "OK");
                }


            }

        }
    }
}