using System;
using System.Collections.Generic;
using exampleproject1.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace exampleproject1
{
    public partial class ScanBarcode : ContentPage
    {
        public ScanBarcode()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                InitializeComponent();
                NavigationPage.SetBackButtonTitle(this, "");
            }
            //Empty string as title
        }

        string result2;
        void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
                {

                    scanResultText.Text = result.Text;
                    result2 = result.Text;
                    if (!string.IsNullOrWhiteSpace(result2))
                    {
                       
                        BarcodeScanner.isScanned = true;
                        MessagingCenter.Send<object, string>(this, "ScannedBarcode", result2);

                        _ = Navigation.PopAsync();
                    }
                    BarcodeScanner.Barcode = result.Text;
                    //scanResultText.Text = result.Text + " (type: " + result.BarcodeFormat.ToString() + ")";
                });
            
        }

       
    }
}
