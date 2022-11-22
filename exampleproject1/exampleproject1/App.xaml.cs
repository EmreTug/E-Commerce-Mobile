using exampleproject1.Helpers;
using exampleproject1.Models;
using exampleproject1.Services;
using exampleproject1.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("NunitoSans-Regular.ttf", Alias = "ThemeFontRegular")]
[assembly: ExportFont("NunitoSans-SemiBold.ttf", Alias = "ThemeFontMedium")]
[assembly: ExportFont("NunitoSans-Bold.ttf", Alias = "ThemeFontBold")]
namespace exampleproject1
{
  
    public partial class App : Application
    {
        public App()
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


                if (check().Result)
                {
                    Application.Current.MainPage = new NavigationPage(new MainPage());

                }
                else
                {
                    MainPage = new NavigationPage(new Login());

                }
            }


        }

        public async Task<bool> check()
        {
            string accesstoken = "";

            if (Settings.Username != "" && Settings.Password != "")
            {
                if (Settings.AccessToken != "")
                {
                    accesstoken = await AdminServices.LoginCheckAsync(Settings.Username.ToString(), Settings.Password.ToString()).ConfigureAwait(false);

                    string x = await AdminServices.CreateToken();
                    var d = await AdminServices.updateToken(accesstoken, x);
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
                    return true;

                }
                else
                {
                    Settings.AccessToken = "";
                    return false;
                }

            }
            else
            {
                Settings.AccessToken = "";
                return false;
            }


         
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }
      
    
        protected override void OnResume()
        {
        }

       
    }
}
