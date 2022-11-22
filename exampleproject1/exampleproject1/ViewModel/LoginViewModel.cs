using exampleproject1.Helpers;
using exampleproject1.Models;
using exampleproject1.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace exampleproject1.ViewModel
{

    public class LoginViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

      

        public string Username { get; set; }

        public string Password { get; set; }
        private string _note;
        public string Note
        {
            get
            {
                return _note;
            }

            set
            {
                _note = value; 

                OnPropertyChanged("Note");
            }
        }


        public ICommand LoginCommand
        {
            get
            {

                string accesstoken = "";
                return new Command(async () =>
                {
                    var current = Connectivity.NetworkAccess;

                    if (current == NetworkAccess.None)
                    {

                        Note = "İnternet Bağlantınızı Kontrol Edin";

                    }
                    else
                    {
                        if (Username != "" && Password != "")
                    {
                        accesstoken = await AdminServices.LoginCheckAsync(Username.ToString(), Password.ToString());
                        if (accesstoken != "")
                        {
                            string x = await AdminServices.CreateToken();
                            var d = await AdminServices.updateToken(accesstoken, x);
                            Settings.AccessToken = x;
                            Settings.Username = Username;
                            Settings.Password = Password;

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

                            Note = "";

                            Application.Current.MainPage = new NavigationPage(new MainPage());

                    }
                }

                });
            }
        }

        public LoginViewModel()
        {
            Username = Settings.Username;
            Password = Settings.Password;
        }

    }
}
