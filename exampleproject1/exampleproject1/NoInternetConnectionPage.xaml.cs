using exampleproject1.Helpers;
using exampleproject1.Models;
using exampleproject1.Services;
using exampleproject1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace exampleproject1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoInternetConnectionPage : ContentPage
    {
        public NoInternetConnectionPage()
        {
            InitializeComponent();
            
        }

        private async void networkControl(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                await App.Current.MainPage.DisplayAlert("Network Error", "İnternet Bağlantınızı Kontrol Edin", "Ok");

            }
            else
            {
                if (check().Result)
                {
                    Application.Current.MainPage = new NavigationPage(new MainPage());

                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(new Login());


                }
            }
        }

        public async Task<bool> check()
        {
            string accesstoken = "";
            if (Settings.Username != "" && Settings.Password != "")
            {
                accesstoken = await AdminServices.LoginCheckAsync(Settings.Username.ToString(), Settings.Password.ToString()).ConfigureAwait(false);
                if (accesstoken != "")
                {
                    string x = await AdminServices.CreateToken();
                    var d = await AdminServices.updateToken(accesstoken, x).ConfigureAwait(false);
                    Settings.AccessToken = x;

                    TempOrderListModel model = await OrderServices.getTempOrder(Settings.AccessToken);
                    StaticClass.OrderLineList = new System.Collections.ObjectModel.ObservableCollection<OrderLineProductModel>();
                    if (model != null)
                    {
                        foreach (var orderline in model.OrderLine)
                        {
                            if (orderline.ProductVariantGroupId != null)
                            {
                                ProductListModel m = await ProductServices.ProductById(orderline.ProductId);
                                ProductVariantGroupModel pvgm = m.Variants.Find(r => r.Id == orderline.ProductVariantGroupId);
                                string desc = "";
                                foreach (var c in pvgm.VariantNames)
                                {
                                    desc = desc + " " + c.VariantValue;
                                }
                                StaticClass.OrderLineList.Add(new OrderLineProductModel
                                {
                                    ProductId = orderline.ProductId,
                                    ProductVariantGroupId = orderline.ProductVariantGroupId,
                                    Quantity = orderline.Quantity,
                                    UnitPrice = orderline.UnitPrice,
                                    date = orderline.date,
                                    Description = desc.ToUpper(),
                                    image = m.Variants.Find(r => r.Id == orderline.ProductVariantGroupId).Images.FirstOrDefault(),
                                    stock = m.Variants.Find(r => r.Id == orderline.ProductVariantGroupId).Stock,
                                    TotalPrice = orderline.Quantity * orderline.UnitPrice,
                                    ProductName = m.Name


                                });
                            }
                            else
                            {
                                ProductListModel m = await ProductServices.ProductById(orderline.ProductId);
                                string desc = m.Description;
                                StaticClass.OrderLineList.Add(new OrderLineProductModel
                                {
                                    ProductId = orderline.ProductId,
                                    Quantity = orderline.Quantity,
                                    UnitPrice = orderline.UnitPrice,
                                    date = orderline.date,
                                    Description = desc.ToUpper(),
                                    image = m.firstImage,
                                    stock = m.Stock,
                                    TotalPrice = orderline.Quantity * orderline.UnitPrice,
                                    ProductName = m.Name


                                });
                            }


                        }

                        ProductViewModel.TempOrderlineList = StaticClass.OrderLineList;
                        StaticClass.Instance.TotalPrice = 0;
                        foreach (var price in StaticClass.OrderLineList)
                        {
                            StaticClass.Instance.TotalPrice += price.TotalPrice;

                        }
                    }

                }

            }
            else
            {
                Settings.AccessToken = "";
            }


            if (Settings.AccessToken != "" && accesstoken != null)
            {

                return true;


            }
            else
            {
                return false;
            }
        }

    }
}