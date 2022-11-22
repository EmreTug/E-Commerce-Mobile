using exampleproject1.customeRenderer;
using exampleproject1.Models;
using exampleproject1.Services;
using exampleproject1.ViewModel;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace exampleproject1
{
    public partial class OrderBasket : ContentPage
    {



        public OrderBasket()
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

               

                sepet.ItemsSource = StaticClass.OrderLineList;

                if (StaticClass.OrderLineList.Count < 1)
                {
                    OrderSummary.IsVisible = false;
                    sepet.Margin = new Thickness(0, 0, 0, 0);
                }
                else if (StaticClass.OrderLineList.Count >= 1)
                {
                    OrderSummary.IsVisible = true;
                    sepet.Margin = new Thickness(0, 0, 0, 100);

                }
            }
           

            price.Text = Math.Round(StaticClass.Instance.TotalPrice,2).ToString()+" TL";
        }

        private void increase(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                price.Text = Math.Round(StaticClass.Instance.TotalPrice, 2).ToString() + " TL";
            }
        }

        private void decrease(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            var c = StaticClass.Deneme1;
            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                price.Text = Math.Round(StaticClass.Instance.TotalPrice, 2).ToString() + " TL";
                if (StaticClass.OrderLineList.Count < 1)
                {
                    sepet.Margin = new Thickness(0, 0, 0, 0);
                    OrderSummary.IsVisible = false;
                }
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
       
        public void staticdeleteProduct()
        {
            price.Text = Math.Round(StaticClass.Instance.TotalPrice, 2).ToString() + " TL";

            if (StaticClass.OrderLineList.Count < 1)
            {
                sepet.Margin = new Thickness(0, 0, 0, 0);
                OrderSummary.IsVisible = false;
            }
        }
        private void deleteProduct(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                price.Text = Math.Round(StaticClass.Instance.TotalPrice, 2).ToString() + " TL";

                if (StaticClass.OrderLineList.Count < 1)
                {
                    sepet.Margin = new Thickness(0, 0, 0, 0);
                    OrderSummary.IsVisible = false;
                }
            }
        }

        private async void Completed(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                MyCustomEntry entry = (MyCustomEntry)sender;
                OrderLineProductModel tempmodel = (OrderLineProductModel)entry.ReturnCommandParameter;
                if (tempmodel.Quantity > tempmodel.stock)
                {
                    tempmodel.Text = "Maximum Stok " + tempmodel.stock;
                    tempmodel.Quantity = (long)tempmodel.stock;
                    entry.Text = tempmodel.stock.ToString();
                    TempOrderAddModel model = new TempOrderAddModel();
                    List<TempOrderLineAddModel> moddel = new List<TempOrderLineAddModel>();
                    foreach (var item in StaticClass.OrderLineList)
                    {
                        moddel.Add(new TempOrderLineAddModel
                        {

                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            date = DateTime.Now.ToString(),
                            ProductId = item.ProductId,
                            ProductVariantGroupId = item.ProductVariantGroupId,

                        });

                    }
                    model.OrderLine = moddel;

                    await OrderServices.updateTempOrder(model);
                    StaticClass.Instance.TotalPrice = 0;
                    foreach (var price in model.OrderLine)
                    {
                        StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
                        StaticClass.Instance.TotalPrice += Math.Round(price.Quantity * price.UnitPrice, 2);

                    }
                    price.Text = Math.Round(StaticClass.Instance.TotalPrice, 2).ToString();


                }
                else if (tempmodel.Quantity < 1)
                {
                    tempmodel.Text = "";
                    tempmodel.Quantity = 1;
                    TempOrderAddModel model = new TempOrderAddModel();
                    List<TempOrderLineAddModel> moddel = new List<TempOrderLineAddModel>();
                    foreach (var item in StaticClass.OrderLineList)
                    {
                        moddel.Add(new TempOrderLineAddModel
                        {

                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            date = DateTime.Now.ToString(),
                            ProductId = item.ProductId,
                            ProductVariantGroupId = item.ProductVariantGroupId,

                        });

                    }
                    model.OrderLine = moddel;

                    await OrderServices.updateTempOrder(model);
                    StaticClass.Instance.TotalPrice = 0;
                    foreach (var price in model.OrderLine)
                    {
                        StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
                        StaticClass.Instance.TotalPrice += Math.Round(price.Quantity * price.UnitPrice, 2);

                    }
                    price.Text = Math.Round(StaticClass.Instance.TotalPrice, 2).ToString();

                }
                else
                {
                    tempmodel.Text = "";
                    TempOrderAddModel model = new TempOrderAddModel();
                    List<TempOrderLineAddModel> moddel = new List<TempOrderLineAddModel>();
                    foreach (var item in StaticClass.OrderLineList)
                    {
                        moddel.Add(new TempOrderLineAddModel
                        {

                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            date = DateTime.Now.ToString(),
                            ProductId = item.ProductId,
                            ProductVariantGroupId = item.ProductVariantGroupId,

                        });

                    }
                    model.OrderLine = moddel;

                    await OrderServices.updateTempOrder(model);
                    StaticClass.Instance.TotalPrice = 0;
                    foreach (var price in model.OrderLine)
                    {
                        StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
                        StaticClass.Instance.TotalPrice += Math.Round(price.Quantity * price.UnitPrice, 2);

                    }
                    price.Text = Math.Round(StaticClass.Instance.TotalPrice, 2).ToString();

                }
            }
        }

        private async void getConfirmOrder(object sender, EventArgs e)
        {
            var c = StaticClass.Deneme1;
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                await Navigation.PushAsync(new ConfirmOrder());
            }

        }
    }

}
