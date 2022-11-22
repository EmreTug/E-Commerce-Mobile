using exampleproject1.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Collections.Generic;
using ZXing.Net.Mobile.Forms;
using exampleproject1.ViewModel;
using exampleproject1.Helpers;
using Xamarin.Essentials;
using static exampleproject1.MainPage;
using Rg.Plugins.Popup.Services;

namespace exampleproject1
{

    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MainPage : ContentPage, IHasCollectionView
    {
        public CollectionView CollectionView => CollectionViewFilters;



        protected override void OnBindingContextChanged()
        {
            if (this.BindingContext is IHasCollectionViewModel hasCollectionViewModel)
            {
                hasCollectionViewModel.View = this;
            }
            base.OnBindingContextChanged();
        }


        public static string selectedProductBarcode;

        public MainPage()
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
          

            // getData();


        }
        public static Action BackPressed;
        
        private bool AcceptBack;
        protected override bool OnBackButtonPressed()
        {
            if (AcceptBack)
                return false;

            PromptForExit();
            return true;
        }

        private async void PromptForExit()
        {
            if (await DisplayAlert("", "Are you sure to exit?", "Yes", "No"))
            {
                AcceptBack = true;
                BackPressed();
            }

        }

        //sayfadaki ürünleri getiren fonksiyon
        int currentDeviceHeight = 0;
       

        //sayfadaki ürünleri getiren fonksiyon
//        public List<ProductListModel> AllProducts = new List<ProductListModel>();
        public List<ProductListModel> basketProducts = new List<ProductListModel>();
        public List<ProductListModel> EmptyBasketProducts = new List<ProductListModel>();
        List<ProductListModel> scanedProduct = new List<ProductListModel>();







      
        protected override void OnAppearing()
        {
            base.OnAppearing();
            currentDeviceHeight = (int)this.Height;
            StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
            TotalPrice.Text = StaticClass.Instance.TotalPrice.ToString() + " TL";

            // currentDeviceHeight = (int)this.Height;


            //MessagingCenter.Subscribe<object,string>(this, "ScannedBarcode", (s,e) => {
            //    scanedProduct = BarcodeScanner.CurrentAllProducts.FindAll(x => x.Barcode == e);

            //    scanedProduct[0].Count = scanedProduct.Count + 1;

            //    frameOrderSummary.IsVisible = true;


            //ListViewOrders.ItemsSource = scanedProduct;


            //});

        }




        uint duration = 100;
        double openY = (Device.RuntimePlatform == "İOS") ? 850 : 60;
        double LastPanY = 0;
        bool isBackdropTapEnabled = true;

        //ekranın farklı bir yerine dokununca popup ı kapatan fonksiyon
        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {

            if (isBackdropTapEnabled)
            {
                await OpenDrawer();
            }
        }

      
        async void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {

            if (e.StatusType == GestureStatus.Running)
            {
                isBackdropTapEnabled = false;
                LastPanY = e.TotalY;
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

        //popup üzerinde tıklama yapınca çalışacak fonksiyon
        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            if (bglayout.IsVisible == false)
            {
                await CloseDrawer();
            }
            else
            {
                await OpenDrawer();
            }
        }

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            Label lbl = (sender as Label);
            var emre = lbl.Text;
        }






        //popup açılış

        async Task CloseDrawer()
        {
            StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
            TotalPrice.Text = StaticClass.Instance.TotalPrice.ToString() + " TL";

            bglayout.IsVisible = false;

            await Task.WhenAll
            (

                Backdrop.FadeTo(1, length: duration),
                BottomToolbar.TranslateTo(0, currentDeviceHeight, length: 250, easing: Easing.SinIn)
            );
           // ListProducts.ItemsSource = null;

        }
        //popup kapanış
        async Task OpenDrawer()
        {
           


            bglayout.IsVisible = true;


            await Task.WhenAll
            (
                Backdrop.FadeTo(0, length: duration),
                BottomToolbar.TranslateTo(0, currentDeviceHeight - 280, length: 250, easing: Easing.SinIn)
            );
        }




        //To put the products according to oriantation
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);


            if (CurrentDevice.IsOrientationPortrait() && width > height || !CurrentDevice.IsOrientationPortrait() && width < height)
            {
                int split;
                CurrentDevice.SetSize(width, height);

                // Orientation got changed! Do your changes here
                if (CurrentDevice.IsOrientationPortrait())
                {
                    // portrait mode
                    split = 3;
                }

                else
                {
                    // landscape mode

                    split = 6;

                }

                var layout = (GridItemsLayout)collectionView.ItemsLayout;
                layout.Span = split;
                collectionView.ItemsLayout = layout;
            }


        }



       


       async void CheckVariantPopUp(System.Object sender, System.EventArgs e)
        {
            StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
            TotalPrice.Text = StaticClass.Instance.TotalPrice.ToString() + " TL";
            await CloseDrawer();
        }






           

        private async void ClickedConfirmOrder(object sender, EventArgs e)
        {
           
            await Navigation.PushAsync(new OrderBasket());
        }

      
      



        //Sipariş  özeti kısmının açılması
        private void ClickedOrderSummary(object sender, EventArgs e)
        {
            ListViewOrders.ItemsSource = StaticClass.OrderLineList; 
            if (frameOrderSummary.IsVisible)
            {
                frameOrderSummary.IsVisible = false;
                iconDown.Source = "icon_down.png";

            }
            else
            {
             
                    frameOrderSummary.IsVisible = true;
                    iconDown.Source = "icon_up.png";

                
            }
        }

        //Filtreler bölümünün açılması
        private void ClickedFilterSection(object sender, EventArgs e)
        {
            if (frameVisible.IsVisible == true)
            {
                frameVisible.IsVisible = false;
            }

            else
            {
                frameVisible.IsVisible = true;
            }
        }



        async private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            if (isBackdropTapEnabled)
            {
                await CloseDrawer();
            }
        }



       



        //seçilen ürüne göre popup açan fonksiyon
        public static ProductListModel productPopUp;


        async void SelectedProduct(object sender, EventArgs e)
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



        

       

        void ClickedSelectFilter(System.Object sender, System.EventArgs e)
        {
        }



        async void CheckProductPopUp(System.Object sender, System.EventArgs e)
        {
            OnAppearing();
            Frame p = (Frame)sender;
            var tapGesture = (TapGestureRecognizer)p.GestureRecognizers[0];
            var item = (ProductListModel)tapGesture.CommandParameter;
            if (item.Variants.Count>0)
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
                TotalPrice.Text = (Math.Round(StaticClass.Instance.TotalPrice,2) + Math.Round(item.Price,2)).ToString() + " TL";
                _ = DisplayAlert("Başarılı", "Ürün Sepete Eklendi", "OK");

            }
        }
        async void CheckProductDetailPage(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ProductsDetail());

        }

        private void signout(object sender, EventArgs e)
        {
            StaticClass.Instance.TotalPrice = 0;
            StaticClass.AllProducts = new System.Collections.ObjectModel.ObservableCollection<ProductListModel>();
            StaticClass.productId = 0;
            Settings.AccessToken = "";
     
            Application.Current.MainPage = new NavigationPage(new Login());

        }

        void OpenSignoutPopUp(System.Object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SignoutAlert());
        }
    }

}

public class CurrentDevice
{
    protected static CurrentDevice Instance;
    double width;
    double height;

    static CurrentDevice()
    {
        Instance = new CurrentDevice();
    }
    protected CurrentDevice()
    {
    }

    public static bool IsOrientationPortrait()
    {
        return Instance.height > Instance.width;
    }

    public static void SetSize(double width, double height)
    {
        Instance.width = width;
        Instance.height = height;
    }

}