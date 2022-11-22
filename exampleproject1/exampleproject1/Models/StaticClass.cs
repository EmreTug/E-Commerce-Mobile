using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static exampleproject1.Models.ProductModels;

namespace exampleproject1.Models
{
    public class StaticClass: INotifyPropertyChanged
    {

        private static ObservableCollection<ProductListModel> _allProducts;
        public static ObservableCollection<ProductListModel> AllProducts
        {
            get
            {
                if (_allProducts == null)
                {
                    _allProducts = new ObservableCollection<ProductListModel>();

                }
                return _allProducts;
            }
            set
            {
                _allProducts = value;

            }
        }
        private double _totalPrice;

        public double TotalPrice
        {
            get
            {
                return _totalPrice;
            }

            set
            {
                _totalPrice = value;
                OnPropertyChanged("TotalPrice");

            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");

            }
        }
        private bool _isBusy2;

        public bool IsBusy2
        {
            get
            {
                return _isBusy2;
            }
            set
            {
                _isBusy2 = value;
                OnPropertyChanged("IsBusy2");

            }
        }
        private bool _isBusy3;

        public bool IsBusy3
        {
            get
            {
                return _isBusy3;
            }
            set
            {
                _isBusy3 = value;
                OnPropertyChanged("IsBusy3");

            }
        }
        static private ObservableCollection<FilterModel> _filters;
        static public ObservableCollection<FilterModel> Filters
        {
            get
            {
                if (_filters == null)
                {
                    _filters = new ObservableCollection<FilterModel>();

                }
                return _filters;
            }

            set
            {
                _filters = value;
            }


        }

        static private ObservableCollection<string> _deneme1;
        static public ObservableCollection<string> Deneme1
        {
            get
            {
                if (_deneme1 == null)
                {
                    _deneme1 = new ObservableCollection<string>();

                }
                return _deneme1;
            }

            set
            {
                _deneme1 = value;
            }


        }

        static private GetProductModel _pagination;

        static public GetProductModel pagination
        {
            get
            {
                if (_pagination == null)
                {
                    _pagination = new GetProductModel();

                }
                return _pagination;
            }
            set
            {
                
                _pagination = value;



            }
        }
        static private ObservableCollection<OrderLineProductModel> _orderLineList;
        public static ObservableCollection<OrderLineProductModel> OrderLineList
        {
            get
            {
                if (_orderLineList == null)
                {
                    _orderLineList = new ObservableCollection<OrderLineProductModel>();

                }
                return _orderLineList;
            }
            set
            {
                _orderLineList = value;

            }
        }
        static public int checkProductLoad { get; set; } = 0;
        static public long productId { get; set; }
        
        




        //private ctor so you need to use the Instance prop
        private StaticClass() {
           
        }

        private static StaticClass _instance;

        public static StaticClass Instance
        {
            get { return _instance ?? (_instance = new StaticClass()); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }


    
}
