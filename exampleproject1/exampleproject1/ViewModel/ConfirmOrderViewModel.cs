using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using exampleproject1.Helpers;
using exampleproject1.Models;
using exampleproject1.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace exampleproject1.ViewModel
{
    public class ConfirmOrderViewModel : INotifyPropertyChanged
    {



        public ConfirmOrderViewModel()
        {
            confirmationOrder = new Command(onConfirmOrder);
        }

        ICommand confirmationOrder;

        public ICommand ConfirmationOrder
        {
            get
            {
                return confirmationOrder;
            }

            set
            {
                confirmationOrder = value;
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name != value) { _name = value; }

                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                if (_lastName != value) { _lastName = value; }

                OnPropertyChanged();
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }

            set
            {
                if (_phoneNumber != value) { _phoneNumber = value; }

                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                if (_email != value) { _email = value; }

                OnPropertyChanged();
            }
        }


        private string _city;
        public string City
        {
            get
            {
                return _city;
            }

            set
            {
                if (_city != value) { _city = value; }

                OnPropertyChanged();
            }
        }


        private string _town;
        public string Town
        {
            get
            {
                return _town;
            }

            set
            {
                if (_town != value) { _town = value; }

                OnPropertyChanged();
            }
        }

        private string _address;
        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                if (_address != value) { _address = value; }

                OnPropertyChanged();
            }
        }

        private string _taxName;
        public string TaxName
        {
            get
            {
                return _taxName;
            }

            set
            {
                if (_taxName != value) { _taxName = value; }

                OnPropertyChanged();
            }
        }


        private string _taxNo;
        public string TaxNo
        {
            get
            {
                return _taxNo;
            }

            set
            {
                if (_taxNo != value) { _taxNo = value; }

                OnPropertyChanged();
            }
        }


        //Display alert textleri
        string DisplayAlertTitle = "";
        string DisplayAlertDesctription = "";

       //Tüm bilgiler için kontrol saglama 
        private bool CheckFields()
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(LastName) &&
                !string.IsNullOrEmpty(City) && !string.IsNullOrEmpty(Town) && !string.IsNullOrEmpty(Address)
                && !string.IsNullOrEmpty(TaxName) && !string.IsNullOrEmpty(TaxNo))
            {
                if (IsPhoneNumber(PhoneNumber))
                {
                    if (IsValidEmail(Email))
                    {
                        return true;
                    }
                    else
                    {
                        DisplayAlertTitle = "E-mail adresiniz hatalı";
                        DisplayAlertDesctription = "Sipariş verebilmek için geçerli bir e-mail adresi giriniz.";
                        return false;
                    }
                }
                else
                {
                    DisplayAlertTitle = "Telefon numaranız hatalı";
                    DisplayAlertDesctription = "Sipariş verebilmek için geçerli bir telefon numarası giriniz.";
                    return false;
                }
                
                
            }
            else
            {
                DisplayAlertTitle = "Boş alanları doldurunuz";
                DisplayAlertDesctription = "Sipariş verebilmek için tüm bilgileri eksiksiz giriniz.";
                return false;
            }
        }



        //Girilen e-mail formatı dogru mu
        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^[0-9-]{12}$").Success;

            //return Regex.Match(number, @"^[01-9][0-9]{10}").Success;
        }

        private bool IsValidPhoneNumber(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private void onConfirmOrder(object param)
        {

            _ = onConfirmOrderAsync(param);
        }


        //Siparis olusturma
        private async Task onConfirmOrderAsync(object param)
        {
            StaticClass.Instance.IsBusy2 = true;
            StaticClass.Instance.IsBusy3 = false;

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                var check = CheckFields();
                if (check)
                {
                    
                    List<OrderLineAddModel> ModelOrderLineList = new List<OrderLineAddModel>();

                    OrderAddModel ModelOrder = new OrderAddModel();
                    ModelOrder.CustomerName = Name;
                    ModelOrder.CustomerLastName = LastName;
                    ModelOrder.CustomerPhoneNumber = PhoneNumber;
                    ModelOrder.CustomerMailAddress = Email;
                    ModelOrder.CustomerAddress = Address;
                    ModelOrder.CustomerTaxAdministration = TaxName;
                    ModelOrder.CustomerTaxId = Convert.ToInt64(TaxNo);
                    ModelOrder.OrderDate = DateTime.Now.ToString();



                    foreach (var item in StaticClass.OrderLineList)
                    {
                        ModelOrderLineList.Add(new OrderLineAddModel { ProductId = item.ProductId, ProductVariantGroupId = item.ProductVariantGroupId, DiscountRate = 0, Quantity = item.Quantity, UnitPrice = item.UnitPrice });

                    }
                    ModelOrder.OrderLine.AddRange(ModelOrderLineList);
                    await OrderServices.AddOrder(ModelOrder);
                    foreach (var item in ModelOrderLineList)
                    {
                        var x = await OrderServices.deleteTempOrder(item.ProductId, item.ProductVariantGroupId);

                    }
                    StaticClass.Instance.TotalPrice = 0;
                    StaticClass.OrderLineList = new System.Collections.ObjectModel.ObservableCollection<OrderLineProductModel>();
                    await Application.Current.MainPage.DisplayAlert("Başarılı", "Siparişiniz Alındı", "OK");
                    var s = StaticClass.Filters;
                    await Application.Current.MainPage.Navigation.PopToRootAsync();

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(DisplayAlertTitle, DisplayAlertDesctription, "OK");

                }




            }
           
            StaticClass.Instance.TotalPrice = 0;
            StaticClass.Instance.IsBusy2 = false;
            StaticClass.Instance.IsBusy3 = true;


        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }


    }
}
