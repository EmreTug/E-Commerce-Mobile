using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using exampleproject1.Models;
using exampleproject1.Services;
using Xamarin.Forms;
using System.Linq;
using static exampleproject1.Models.ProductModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZXing.Net.Mobile.Forms;
using System.Linq.Expressions;
using System.Reflection;
using exampleproject1.Helpers;
using Xamarin.Essentials;



//Hamiyet Deneme


namespace exampleproject1.ViewModel
{
    public class ProductViewModel : INotifyPropertyChanged, IHasCollectionViewModel
    {


      
        public IHasCollectionView View { get; set; }
        // use CollectionView like
        private void ScrollToItem(int index)
        {
            View.CollectionView.ScrollTo(index); // don't forget check null
        }



        public ProductViewModel()
        {
            _ = LoadFiltersAsync();
            _ = LoadProductsAsync();
            add();
            IsProductSelected = false;
            remainingItemsThresholdReached = new Command(onremainingItemsThresholdReached);
            selectProductDetail = new Command(onProductDetail);
            decreasee = new Command(ondecrease);
            increasee = new Command(onincrease);
            deleteProduct = new Command(onDeleteProduct);
            selectFilter = new Command(onSelectFilter);
            selectProduct = new Command(onSelectProduct);
            selectVariant = new Command(onSelectVariant);
            addBasket = new Command(onAddBasket);
            scanBarcode = new Command(onScanBarcode);


        }
       
        public void add()
        {
            StaticClass.Deneme1.Add("bir");
        }

        //***********************************************************************************************************************************

        public static List<long> selectedF = new List<long>();


        private static bool _isBusy;
        public static bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;

            }
        }








        private bool _isProductSelected;
        public bool IsProductSelected
        {
            get
            {
                return _isProductSelected;
            }
            set
            {
                _isProductSelected = value;
                OnPropertyChanged("IsProductSelected");
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

        private ObservableCollection<ProductListModel> _selectedFilterProducts;
        public ObservableCollection<ProductListModel> SelectedFilterProducts
        {
            get
            {
                if (_selectedFilterProducts == null)
                {
                    _selectedFilterProducts = new ObservableCollection<ProductListModel>();

                }
                return _selectedFilterProducts;
            }

            set
            {
                _selectedFilterProducts = value;
            }


        }


      





        //Genel tüm ürünleirn tutulduğu liste
        private static ObservableCollection<ProductListModel> _allProducts = null;
        public ObservableCollection<ProductListModel> AllProducts
        {
            get
            {

                return StaticClass.AllProducts;


            }
            set
            {
                _allProducts.Distinct();
                _allProducts = value;
               
            }


        }



        private List<OrderLineProductModel> _orderLine;
        public List<OrderLineProductModel> OrderLine
        {
            get
            {
                if (_orderLine == null)
                {
                    _orderLine = new List<OrderLineProductModel>();

                }
                return _orderLine;
            }

            set
            {
                _orderLine = value;
            }

        }


        private ObservableCollection<FilterModel> _filters;
        public ObservableCollection<FilterModel> Filters
        {
            get
            {
                
                return StaticClass.Filters;
            }

            set
            {
                _filters = value;
            }


        }


        private ObservableCollection<GroupedVariantHeaderModel> _try;
        public ObservableCollection<GroupedVariantHeaderModel> Try
        {
            get
            {
                if (_try == null)
                {
                    _try = new ObservableCollection<GroupedVariantHeaderModel>();

                }
                return _try;
            }

            set
            {
                _try = value;
            }


        }


        private List<VariantNameModel> _variantList;
        public List<VariantNameModel> VariantList
        {
            get
            {
                if (_variantList == null)
                {
                    _variantList = new List<VariantNameModel>();

                }
                return _variantList;
            }

            set
            {
                _variantList = value;
            }

        }


        private static List<VariantNameModel> _deneme;
        public static List<VariantNameModel> Deneme
        {
            get
            {
                if (_deneme == null)
                {
                    _deneme = new List<VariantNameModel>();

                }
                return _deneme;
            }

            set
            {
                _deneme = value;
            }

        }


        private ObservableCollection<GroupedVariantHeaderModel> _newsItem;
        public ObservableCollection<GroupedVariantHeaderModel> NewsItem
        {
            get
            {
                if (_newsItem == null)
                {
                    _newsItem = new ObservableCollection<GroupedVariantHeaderModel>();

                }
                return _newsItem;
            }
            set
            {
                _newsItem = value;

            }
        }

        private async void ondecrease(object param)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
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

                OrderLineProductModel selectedVariant = (OrderLineProductModel)param;
                if (selectedVariant.ProductVariantGroupId > 0)
                {
                    if (selectedVariant.Quantity > 1)
                    {
                        StaticClass.OrderLineList.FirstOrDefault(v => v.ProductVariantGroupId == selectedVariant.ProductVariantGroupId).Text = "";

                        model.OrderLine.FirstOrDefault(v => v.ProductVariantGroupId == selectedVariant.ProductVariantGroupId).Quantity--;
                        selectedVariant.Quantity--;
                        selectedVariant.TotalPrice = selectedVariant.Quantity * selectedVariant.UnitPrice;
                        StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
                        StaticClass.Instance.TotalPrice -= selectedVariant.UnitPrice;

                        await OrderServices.updateTempOrder(model);

                    }
                    else
                    {
                        StaticClass.OrderLineList.FirstOrDefault(v => v.ProductVariantGroupId == selectedVariant.ProductVariantGroupId).Text = "";

                        selectedVariant.Quantity--;
                        TempOrderlineList.Remove(selectedVariant);
                        StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
                        StaticClass.Instance.TotalPrice -= selectedVariant.UnitPrice;



                        await OrderServices.deleteTempOrder(selectedVariant.ProductId, selectedVariant.ProductVariantGroupId);
                        StaticClass.OrderLineList.Remove(selectedVariant);




                    }

                }
                else
                {
                    if (selectedVariant.Quantity > 1)
                    {
                        StaticClass.OrderLineList.FirstOrDefault(v => v.ProductId == selectedVariant.ProductId).Text = "";

                        model.OrderLine.FirstOrDefault(v => v.ProductId == selectedVariant.ProductId).Quantity--;
                        selectedVariant.Quantity--;
                        selectedVariant.TotalPrice = selectedVariant.Quantity * selectedVariant.UnitPrice;
                        StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
                        StaticClass.Instance.TotalPrice -= selectedVariant.UnitPrice;

                        await OrderServices.updateTempOrder(model);

                    }
                    else
                    {
                        StaticClass.OrderLineList.FirstOrDefault(v => v.ProductId == selectedVariant.ProductId).Text = "";

                        selectedVariant.Quantity--;
                        TempOrderlineList.Remove(selectedVariant);
                        StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
                        StaticClass.Instance.TotalPrice -= selectedVariant.UnitPrice;



                        await OrderServices.deleteTempOrder(selectedVariant.ProductId, selectedVariant.ProductVariantGroupId);
                        StaticClass.OrderLineList.Remove(selectedVariant);




                    }
                }

            }


        }


        private async void onincrease(object param)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {

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
                        ProductVariantGroupId = item.ProductVariantGroupId

                    });


                }

                OrderLineProductModel selectedVariant = (OrderLineProductModel)param;
                model.OrderLine = moddel;

                if (selectedVariant.Quantity < selectedVariant.stock)
                {
                    model.OrderLine.FirstOrDefault(v => v.ProductVariantGroupId == selectedVariant.ProductVariantGroupId).Quantity++;
                    selectedVariant.Quantity++;
                    selectedVariant.TotalPrice = selectedVariant.Quantity * selectedVariant.UnitPrice;
                    StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
                    StaticClass.Instance.TotalPrice += selectedVariant.UnitPrice;

                    await OrderServices.updateTempOrder(model);


                }
                else
                {
                    StaticClass.OrderLineList.FirstOrDefault(v => v.ProductVariantGroupId == selectedVariant.ProductVariantGroupId).Text = "Maximum Stok " + StaticClass.OrderLineList.FirstOrDefault(v => v.ProductVariantGroupId == selectedVariant.ProductVariantGroupId).stock.ToString();

                }

            }

        }
     

        public static ObservableCollection<OrderLineProductModel> TempOrderlineList = new ObservableCollection<OrderLineProductModel>();






        ICommand selectFilter, selectProduct, selectVariant, addBasket, scanBarcode, selectProductDetail, decreasee, increasee, deleteProduct, remainingItemsThresholdReached;
        public ICommand RemainingItemsThresholdReached
        {
            get
            {
                return remainingItemsThresholdReached;
            }

            set
            {
                remainingItemsThresholdReached = value;
            }
        }
        public ICommand DeleteProduct
        {
            get
            {
                return deleteProduct;
            }

            set
            {
                deleteProduct = value;
            }
        }
        public ICommand SelectProductDetail
        {
            get
            {
                return selectProductDetail;
            }

            set
            {
                selectProductDetail = value;
            }
        }
        public ICommand Decrease
        {
            get
            {
                return decreasee;
            }

            set
            {
                decreasee = value;
            }
        }
        public ICommand Increase
        {
            get
            {
                return increasee;
            }

            set
            {
                increasee = value;
            }
        }
        public ICommand ScanBarcode
        {
            get
            {
                return scanBarcode;
            }

            set
            {
                scanBarcode = value;
            }
        }



        public ICommand SelectFilter
        {
            get
            {
                return selectFilter;
            }

            set
            {
                selectFilter = value;
            }
        }


        public ICommand SelectProduct
        {
            get
            {
                return selectProduct;
            }

            set
            {
                selectProduct = value;
            }
        }


        public ICommand SelectVariant
        {
            get
            {
                return selectVariant;
            }

            set
            {
                if (selectVariant == null)
                    return;
                selectVariant = value;
            }
        }

        public ICommand AddBasket
        {
            get
            {
                return addBasket;
            }

            set
            {
                if (addBasket == null)
                    return;
                addBasket = value;
            }
        }



        /// --------------------------------------------------------------------------------------------------------------------------------
        /// FILTRE SEÇME VE FILTREYE GORE URUN GETIRME - START
        /// --------------------------------------------------------------------------------------------------------------------------------

        private async Task LoadFiltersAsync()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                ObservableCollection<CategoryModel> AllCategories = await ProductServices.AllCategory();

                foreach (var item in AllCategories)
                {

                    FilterModel tempModel = new FilterModel();
                    tempModel.Name = item.CategoryName;
                    tempModel.CategoryId = item.CategoryId;
                    tempModel.IsSelected = false;

                    if (StaticClass.Filters.Any(a=>a.CategoryId==tempModel.CategoryId))
                    {

                    }
                    else
                    {
                        StaticClass.Filters.Add(tempModel);

                    }

                }

            }


        }




        GetProductModel m = new GetProductModel();

        private async Task LoadProductsAsync()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                StaticClass.Instance.IsBusy = true;
                m.Page += 1;

               StaticClass.AllProducts.Clear();
               

                var allProduct = await ProductServices.ProductList(m).ConfigureAwait(false);
                foreach (var item in allProduct)
                {
                    AllProducts.Add(item);

                }
                StaticClass.Instance.IsBusy = false;

                //getPopUpData(AllProducts.FirstOrDefault());
                //Try = NewsItem;
                //var hh = 0;
                //var h = 0;
                //BarcodeScanner.CurrentAllProducts = AllProducts;
            }  }
        public static ProductListModel DetailProduct = new ProductListModel();

        private void onProductDetail(object param)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                ProductListModel selectedProduct = (ProductListModel)param;
                DetailProduct = selectedProduct;
            }




        }

        //deneme
        bool check = true;
        static public bool check2 { get; set; } = true;
        static public bool check3 { get; set; } = true;
        GetProductModel PaginationModelForCategory = new GetProductModel();

        public async void onremainingItemsThresholdReached(object param)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                if (IsBusy2)
                    return;

                //Anasayfa hiç bir filtre seçilmemiş ise
                if (StaticClass.checkProductLoad ==0)
                {

                    if (check)
                    {
                        m.Page += 1;
                        m.Size = 21;

                        IsBusy2 = true;

                        var x = await ProductServices.ProductList(m);
                        if (x.Count > 0)
                        {
                            await Task.Delay(5000);

                            foreach (var item in x)
                            {
                                if (AllProducts.Any(a => a.Barcode == item.Barcode))
                                {

                                }
                                else
                                {
                                    AllProducts.Add(item);
                                }


                            }
                            IsBusy2 = false;

                        }


                        else
                        {
                            IsBusy2 = false;

                            check = false;
                        }
                    }

                }

                //Product detail kısmından filtre seçilmiş ise
                else if(StaticClass.checkProductLoad == 1)
                {
                    if (check2)
                    {
                        StaticClass.pagination.Page += 1;
                        StaticClass.pagination.Size = 21;

                        IsBusy2 = true;

                        var x = await ProductServices.ProductByCategory(StaticClass.pagination, StaticClass.productId);
                        if (x.Count > 0)
                        {
                            await Task.Delay(5000);

                            foreach (var item in x)
                            {
                                if (AllProducts.Any(a => a.Barcode == item.Barcode))
                                {

                                }
                                else
                                {
                                    AllProducts.Add(item);
                                }
                                

                            }
                            IsBusy2 = false;

                        }


                        else
                        {
                            IsBusy2 = false;

                            check2 = false;
                        }
                    }

                }

                //Anasayfadan filtereler kısmından filtre seçilmiş ise
                else if (StaticClass.checkProductLoad == 2)
                {
                    if (check3)
                    {
                        PaginationModelForCategory.Page += 1;
                        PaginationModelForCategory.Size = 21;

                        IsBusy2 = true;

                        var x = await ProductServices.ProductByCategory(PaginationModelForCategory, StaticClass.productId);
                        if (x.Count > 0)
                        {
                            await Task.Delay(5000);

                            foreach (var item in x)
                            {
                                if (AllProducts.Any(a => a.Barcode == item.Barcode))
                                {

                                }
                                else
                                {
                                    AllProducts.Add(item);
                                }


                            }
                            IsBusy2 = false;

                        }


                        else
                        {
                            IsBusy2 = false;

                            check3 = false;
                        }
                    }

                }

            }

        }


        private void onSelectFilter(object param)
        {
            _ = onSelectFilterAsync(param);
        }

        private async Task onSelectFilterAsync(object param)
        {

            FilterModel SelectedFilter = (FilterModel)param;

            StaticClass.checkProductLoad = 2; //load more data kısmı anasayfa filtreler seçiminden geldiğini gösteriyor
            StaticClass.productId = SelectedFilter.CategoryId;
            StaticClass.Instance.IsBusy = true;


            FilterModel temp = new FilterModel();
            //Internet kontrolü
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
               //seçilen kategori filtresini kaldırma
                if (SelectedFilter.IsSelected)
                {
                    SelectedFilter.IsSelected = false;
                    selectedF.Remove(SelectedFilter.CategoryId);

                    //eger hiç bir kategori seçilmemiş ise
                    if (selectedF.Count == 0)
                    {
                        AllProducts.Clear();
                        StaticClass.checkProductLoad = 0;//Load more data kısmı anasayfa da hiç bir filtre uygulanmaması durumunu ifade ediyor

                        m.Page = 1;
                        check = true;
                        PaginationModelForCategory.Page = 0; //Anasayfa filtreler kısmı için sayfalamayı 0 yapma
                        
                       
                        var allProduct = await ProductServices.ProductList(m);
                        foreach (var item in allProduct)
                        {
                            AllProducts.Add(item);

                        }
                        StaticClass.Instance.IsBusy = false;

                    }
                   
                    else
                    {
                        RemoveFilterAsync(SelectedFilter.CategoryId);


                        foreach (var item in StaticClass.Filters.ToList())
                        {
                            if (item.CategoryId == SelectedFilter.CategoryId)
                            {
                                temp = item;
                                StaticClass.Filters.Remove(item);
                            }
                        }

                        StaticClass.Filters.Add(temp);

                        
                        

                    }


                }

                else
                {

                    SelectedFilter.IsSelected = true;
                    PaginationModelForCategory.Page = 0;

                    for (int i = 0; i < StaticClass.Filters.Count; i++)
                    {
                        if (StaticClass.Filters[i].CategoryId == SelectedFilter.CategoryId)
                        {
                            StaticClass.Filters.Insert(0, SelectedFilter);
                            StaticClass.Filters.RemoveAt(i+1);
                        }
                    }
                    ScrollToItem(0);



                    if (!selectedF.Contains(SelectedFilter.CategoryId))
                    {
                        selectedF.Add(SelectedFilter.CategoryId);

                        _ = GetFiltredProductAsync(SelectedFilter.CategoryId);
                    }

                }



            }
            }
        

        private async Task GetFiltredProductAsync(long SelectedFilterName)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else {

                if (selectedF.Count == 1)
                {
                    AllProducts.Clear();

                }

               

                StaticClass.Instance.IsBusy = true;

                GetProductModel PaginationModel = new GetProductModel();
                PaginationModel.Size = 21;
                PaginationModel.Page = 1;

                ObservableCollection<ProductListModel> CategoriesAllProducts = await ProductServices.ProductByCategory(PaginationModel, SelectedFilterName);

                foreach (var item in CategoriesAllProducts)
                {
                    StaticClass.AllProducts.Add(item);
                }
                StaticClass.Instance.IsBusy = false;
            }
            

                

            
        }

        private void RemoveFilterAsync(long removeF)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }


            else
            {
                StaticClass.Instance.IsBusy = true;
                var selected = AllProducts.Where(x => x.Category.BreadCumb.Select(a => a.CategoryId == removeF).FirstOrDefault()).ToList();
                
                foreach (var item in selected)
                {
                    AllProducts.Remove(item);

                }
                StaticClass.Instance.IsBusy = false;

            }
        }

        /// --------------------------------------------------------------------------------------------------------------------------------
        /// FILTRE SEÇME VE FILTREYE GORE URUN GETIRME - END
        /// --------------------------------------------------------------------------------------------------------------------------------



        /// --------------------------------------------------------------------------------------------------------------------------------
        /// BARKOD OKUTMA SAYFASINI AÇMA - START
        /// --------------------------------------------------------------------------------------------------------------------------------
        ZXingScannerPage scanPage;

        string displayAlertMessage;
        string displayAlertDescription;

        private async void onScanBarcode(object param)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                scanPage = new ZXingScannerPage();



                scanPage.OnScanResult += (result) =>
                {
                    scanPage.IsScanning = false;
                //Do something with result

                Device.BeginInvokeOnMainThread(async () =>
                    {


                        _ = addScannedProductAsync(result.Text);
                        await Application.Current.MainPage.Navigation.PopModalAsync();

                        await Application.Current.MainPage.DisplayAlert(displayAlertMessage, displayAlertDescription, "Ok");
                    //DisplayAlert("Scanned Barcode", result.Text, "OK");


                });
                };


                //await Application.Current.MainPage.Navigation.PushModalAsync(scanPage);
                //await Navigation.PushModalAsync(scanPage);



                //?********************
                var toolbarItem = new ToolbarItem { Text = "Cancel" };


                toolbarItem.Clicked += (s, e) =>
                {
                    scanPage.IsScanning = false;
                    Device.BeginInvokeOnMainThread(async () =>
                    {

                        await Application.Current.MainPage.Navigation.PopModalAsync();

                    });
                };

                var navPage = new NavigationPage(scanPage);
                navPage.ToolbarItems.Add(toolbarItem);
                navPage.BarBackgroundColor = Color.Black;
                navPage.BarTextColor = Color.FromHex("#007AFF");
                await Application.Current.MainPage.Navigation.PushModalAsync(navPage);

            }
        }
        /// --------------------------------------------------------------------------------------------------------------------------------
        /// BARKOD OKUTMA SAYFASINI AÇMA - END
        /// --------------------------------------------------------------------------------------------------------------------------------



        /// --------------------------------------------------------------------------------------------------------------------------------
        /// OKUTULAN BARKODU SEPETE EKLEME - START
        /// --------------------------------------------------------------------------------------------------------------------------------
        ProductListModel scanedProduct;


        public async Task addScannedProductAsync(string barcode)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                ProductListModel t =await ProductServices.VariantByBarcode(barcode);
                ProductListModel t1 = await ProductServices.ProductByBarcode(barcode);

                //Taranan barkoda ait ürün yok ise
                if (t == null&&t1==null)
                {

                    displayAlertMessage = "Ürün Bulunamadı";
                    displayAlertDescription = "";

                }

                //Taranan barkoda ait ürün var ise
                else
                {

                    OrderLineProductModel orderLineModel = new OrderLineProductModel();
                    bool check = false;

                    if (t!=null)
                    {

                       

                        displayAlertMessage = "Ürün Sepetinize Eklendi";
                        displayAlertDescription =t.Name;

                        orderLineModel.ProductName = t.Name;
                        orderLineModel.ProductVariantGroupId = t.Variants.FirstOrDefault(v => v.Barcode == barcode).Id;
                        orderLineModel.UnitPrice = (double)t.Variants.FirstOrDefault(v=>v.Barcode==barcode).Price;
                        orderLineModel.TotalPrice = (double)t.Variants.FirstOrDefault(v => v.Barcode == barcode).Price;
                        orderLineModel.ProductId = t.Id;
                        orderLineModel.Description = getVariantValues(t.Variants.FirstOrDefault(v => v.Barcode == barcode)).ToUpper();
                        StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
                        StaticClass.Instance.TotalPrice += orderLineModel.UnitPrice;
                        
                       


                        //Urun daha once sepete eklenmis mi kontrol et
                        foreach (var item in StaticClass.OrderLineList)
                        {
                            if (item.ProductId == t.Id)
                            {
                                if (item.ProductVariantGroupId == t.Variants.FirstOrDefault(v => v.Barcode == barcode).Id)
                                {
                                    //Urun daha once sepete eklenmis miktarını ve toplam fiyatını degistir
                                    item.Quantity = (Convert.ToInt32(item.Quantity) + 1);
                                    item.TotalPrice = (Convert.ToInt32(item.Quantity)) * item.UnitPrice;
                                    check = true;
                                    orderLineModel.TotalPrice = item.TotalPrice;
                                    orderLineModel.Quantity = item.Quantity;
                                }

                            }
                        }
                    }
                    else
                    {

                       

                        displayAlertMessage = "Ürün Sepetinize Eklendi";
                        displayAlertDescription = t1.Name;

                        orderLineModel.ProductName = t1.Name;
                        orderLineModel.UnitPrice = (double)t1.Price;
                        orderLineModel.TotalPrice = (double)t1.Price;
                        orderLineModel.ProductId = t1.Id;
                        orderLineModel.Description = t1.Description.ToUpper();
                        StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
                        StaticClass.Instance.TotalPrice += orderLineModel.UnitPrice;
                      


                        //Urun daha once sepete eklenmis mi kontrol et
                        foreach (var item in StaticClass.OrderLineList)
                        {
                            if (item.ProductId == t1.Id)
                            {
                              
                                    //Urun daha once sepete eklenmis miktarını ve toplam fiyatını degistir
                                    item.Quantity = (Convert.ToInt32(item.Quantity) + 1);
                                    item.TotalPrice = (Convert.ToInt32(item.Quantity)) * item.UnitPrice;
                                    check = true;
                                    orderLineModel.TotalPrice = item.TotalPrice;
                                    orderLineModel.Quantity = item.Quantity;
                                

                            }
                        }
                    }

                    //ürün sepete eklenmemis ilk defa eklenecek
                    if (!check)
                    {
                        orderLineModel.Quantity = 1;

                        StaticClass.OrderLineList.Add(orderLineModel);
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
                                ProductVariantGroupId = item.ProductVariantGroupId

                            });


                        }

                        model.OrderLine = moddel;
                        TempOrderlineList = StaticClass.OrderLineList;
                        TempOrderListModel checkmodel = await OrderServices.getTempOrder(Settings.AccessToken);
                        if (checkmodel != null)
                        {

                            await OrderServices.updateTempOrder(model);
                        }
                        else
                        {
                            await OrderServices.AddTempOrder(model);
                        }
                    }
                    else
                    {
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
                                ProductVariantGroupId = item.ProductVariantGroupId

                            });


                        }

                        model.OrderLine = moddel;
                        TempOrderlineList = StaticClass.OrderLineList;


                        if (OrderServices.getTempOrder(Settings.AccessToken) != null)
                        {
                            await OrderServices.updateTempOrder(model);
                        }
                        else
                        {
                            await OrderServices.AddTempOrder(model);
                        }
                    }





                }
            }
        }



        //Variant valuelerini alma small-red-çizgili
        private string getVariantValues(ProductVariantGroupModel Variants)
        {
            var c = "";

            foreach (var item2 in Variants.VariantNames)
            {
                c = c + " " + item2.VariantValue.ToUpper();
            }


            return c;

        }


    



        /// --------------------------------------------------------------------------------------------------------------------------------
        /// OKUTULAN BARKODU SEPETE EKLEME - END
        /// --------------------------------------------------------------------------------------------------------------------------------


        /// --------------------------------------------------------------------------------------------------------------------------------
        /// SECILEN VARYANTLI URUNU SEPETE EKLEME - START
        /// --------------------------------------------------------------------------------------------------------------------------------
        private async void AddProductBasket()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                ProductListModel product = await ProductServices.ProductById(productPopUp.Id);
                String decription = product.Description.ToUpper();
                bool check = false;
                OrderLineProductModel orderLineModel = new OrderLineProductModel();

                IsProductSelected = true;
                //ProductViewModel selected = (ProductViewModel)param;





                orderLineModel.ProductName = productPopUp.Name;
                orderLineModel.UnitPrice = productPopUp.Price;
                orderLineModel.ProductId = productPopUp.Id;
                orderLineModel.stock = productPopUp.Stock;
                orderLineModel.image = productPopUp.Images.FirstOrDefault();
                orderLineModel.Description = decription;
                orderLineModel.TotalPrice = productPopUp.Price;
                StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
                StaticClass.Instance.TotalPrice += orderLineModel.UnitPrice;





                //Urun daha once sepete eklenmis mi kontrol et
                foreach (var item in StaticClass.OrderLineList)
                {
                    if (item.ProductId == productPopUp.Id)
                    {

                        //Urun daha once sepete eklenmis miktarını ve toplam fiyatını degistir
                        item.Quantity = (Convert.ToInt32(item.Quantity) + 1);
                        item.TotalPrice = (Convert.ToInt32(item.Quantity)) * item.UnitPrice;
                        check = true;
                        orderLineModel.TotalPrice = item.TotalPrice;
                        orderLineModel.Quantity = item.Quantity;




                    }
                }

                //ürün sepete eklenmemis ilk defa eklenecek
                if (!check)
                {
                    orderLineModel.Quantity = 1;

                    StaticClass.OrderLineList.Add(orderLineModel);
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
                    TempOrderlineList = StaticClass.OrderLineList;
                    TempOrderListModel checkmodel = await OrderServices.getTempOrder(Settings.AccessToken);
                    if (checkmodel != null)
                    {

                        await OrderServices.updateTempOrder(model);
                    }
                    else
                    {
                        await OrderServices.AddTempOrder(model);
                    }
                }
                else
                {
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
                    TempOrderlineList = StaticClass.OrderLineList;


                    if (OrderServices.getTempOrder(Settings.AccessToken) != null)
                    {
                        await OrderServices.updateTempOrder(model);
                    }
                    else
                    {
                        await OrderServices.AddTempOrder(model);
                    }
                }

            }

        }

        private async void onAddBasket()
        {

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                bool check = false;
                OrderLineProductModel orderLineModel = new OrderLineProductModel();

                IsProductSelected = true;
                //ProductViewModel selected = (ProductViewModel)param;

                var VariantId = FindVariantId();

                var c2 = "";
                foreach (var item in katmanlar)
                {

                    var c1 = item.VariantValue;

                    var c3 = c1 + " ";
                    c2 += c3;
                }

                tempKatman = katmanlar;
                sonsecim = katmanlar.LastOrDefault().VariantValue;
                var x = getVariantsByKatmans();

                orderLineModel.ProductName = productPopUp.Name;
                orderLineModel.UnitPrice = (double)x.Price;
                orderLineModel.ProductId = productPopUp.Id;
                orderLineModel.ProductVariantGroupId = x.Id;
                orderLineModel.stock = x.Stock;
                orderLineModel.image = x.Images.FirstOrDefault();
                orderLineModel.Description = c2.ToUpper();
                orderLineModel.TotalPrice = (double)x.Price;
                StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
                StaticClass.Instance.TotalPrice += orderLineModel.UnitPrice;



                //Urun daha once sepete eklenmis mi kontrol et
                foreach (var item in StaticClass.OrderLineList)
                {
                    if (item.ProductId == productPopUp.Id)
                    {
                        if (item.ProductVariantGroupId == VariantId)
                        {
                            //Urun daha once sepete eklenmis miktarını ve toplam fiyatını degistir
                            item.Quantity = (Convert.ToInt32(item.Quantity) + 1);
                            item.TotalPrice = (Convert.ToInt32(item.Quantity)) * item.UnitPrice;
                            check = true;
                            orderLineModel.TotalPrice = item.TotalPrice;
                            orderLineModel.Quantity = item.Quantity;



                        }

                    }
                }

                //ürün sepete eklenmemis ilk defa eklenecek
                if (!check)
                {
                    orderLineModel.Quantity = 1;

                    StaticClass.OrderLineList.Add(orderLineModel);
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
                    TempOrderlineList = StaticClass.OrderLineList;
                    TempOrderListModel checkmodel = await OrderServices.getTempOrder(Settings.AccessToken);
                    if (checkmodel != null)
                    {

                        await OrderServices.updateTempOrder(model);
                    }
                    else
                    {
                        await OrderServices.AddTempOrder(model);
                    }
                }
                else
                {
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
                    TempOrderlineList = StaticClass.OrderLineList;


                    if (OrderServices.getTempOrder(Settings.AccessToken) != null)
                    {
                        await OrderServices.updateTempOrder(model);
                    }
                    else
                    {
                        await OrderServices.AddTempOrder(model);
                    }
                }

            }
        }


        public long FindVariantId()
        {

            var variantKatmanList = getVariantsByKatmans();
            if (variantKatmanList == null)
            {
                variantKatmanList = randomVariant;
            }

            return variantKatmanList.Id;

        }

        /// --------------------------------------------------------------------------------------------------------------------------------
        /// SECILEN VARYANTLI URUNU SEPETE EKLEME - END
        /// --------------------------------------------------------------------------------------------------------------------------------




        private async void onDeleteProduct(object param)
        {

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {

                OrderLineProductModel selectedProduct = (OrderLineProductModel)param;
                StaticClass.Instance.TotalPrice = Math.Round(StaticClass.Instance.TotalPrice, 2);
                StaticClass.Instance.TotalPrice -= Math.Round(selectedProduct.UnitPrice * selectedProduct.Quantity, 2);


                TempOrderlineList.Remove(selectedProduct);
                await OrderServices.deleteTempOrder(selectedProduct.ProductId, selectedProduct.ProductVariantGroupId);
                StaticClass.OrderLineList.Remove(selectedProduct);


            }
        }

        /// --------------------------------------------------------------------------------------------------------------------------------
        /// URUNE GORE VARYANTLARI SECME - START
        /// --------------------------------------------------------------------------------------------------------------------------------

        private void onSelectProduct(object param)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                ProductListModel selectedProduct = (ProductListModel)param;
                productPopUp = selectedProduct;
                SelectedProduct(productPopUp);
            }

        }
        void SelectedProduct(ProductListModel SelectedProduct)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                secimler = new List<secimModel>();
                variantNameModels = new List<VariantNameModel>();
                katmanlar = new List<variants>();
                counter = 0;
                getPopUpData(SelectedProduct);
            }

        }

        ProductVariantGroupModel randomVariant = new ProductVariantGroupModel();
        void getPopUpData(ProductListModel product)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                if (product.Variants.Count > 0)
                {
                    var variants = product.Variants.SelectMany(x => x.VariantNames).Select(x => new { VariantName = x.VariantName, VariantValues = x.VariantValue, isSelected = true, bgcolor = "#fff" }).Distinct();
                    List<VariantNameModel> variantNameModel = new List<VariantNameModel>();

                    foreach (var variant in variants)
                    {
                        variantNameModel.Add(new VariantNameModel { VariantName = variant.VariantName, VariantValue = variant.VariantValues });
                    }
                    // NewsItem= variantNameModel.OrderBy(b => b.VariantValue).GroupBy(a => a.VariantName).OrderBy(c => c.Key);

                    randomVariant = product.Variants.FirstOrDefault(a => a.VariantNames != null);
                    var y = randomVariant.VariantNames.OrderBy(a => a.VariantName);
                    getRandomVariantPopUp(y);
                }
                else
                {
                    NewsItem.Clear();
                    AddProductBasket();
                }


            }
        }


        public ProductListModel productPopUp;
        void getRandomVariantPopUp(IOrderedEnumerable<VariantNameModel> randomVariant)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                List<VariantNameModel> eee = new List<VariantNameModel>();
                for (int i = 0; i < randomVariant.ToList().Count; i++)
                {
                    VariantNameModel VariantName = randomVariant.ToList()[i];
                    if (variantNameModels.Count < 1)
                    {
                        var varyants = productPopUp.Variants.SelectMany(x => x.VariantNames).Select(x => new { VariantName = x.VariantName, VariantValues = x.VariantValue }).Distinct();
                        string variantName = varyants.Where(x => x.VariantValues == VariantName.VariantValue).Select(y => y.VariantName).FirstOrDefault();


                        List<string> barkodlar = getVariantBarcodesByVariantValue(VariantName.VariantValue);
                        if (variantNameModels.Count < 1)
                        {
                            foreach (var varyant in varyants)
                            {
                                variantNameModels.Add(new VariantNameModel { VariantName = varyant.VariantName, VariantValue = varyant.VariantValues });
                            }
                        }

                        if (secimler.FirstOrDefault(a => a.VariantName == variantName) == null)
                        {
                            secimler.Add(new secimModel { VariantName = variantName, VariantValue = VariantName.VariantValue });
                        }
                        else if (secimler.FirstOrDefault(a => a.VariantName == variantName && a.VariantValue != VariantName.VariantValue) != null)
                        {
                            secimler.FirstOrDefault(a => a.VariantName == variantName && a.VariantValue != VariantName.VariantValue).VariantValue = VariantName.VariantValue;
                        }
                        else
                        {
                            secimler.Where(a => a.VariantName == variantName).Select(b => b.VariantValue = VariantName.VariantValue);
                        }

                        int index = secimler.IndexOf(secimler.Find(a => a.VariantName == variantName));
                        if (index < 1)
                        {
                            secimler = new List<secimModel>();
                            secimler.Add(new secimModel { VariantName = variantName, VariantValue = VariantName.VariantValue });

                        }
                        int count = secimler.Count;
                        if (katmanlar.Find(a => a.VariantName == variantName) == null)
                        {
                            katmanlar.Add(new variants { VariantName = variantName, katman = counter, isSelected = true, VariantValue = VariantName.VariantValue });
                            counter++;
                        }

                        else
                        {
                            string temp = katmanlar.FirstOrDefault(a => a.VariantName == variantName).VariantValue;


                            katmanlar.FirstOrDefault(a => a.VariantName == variantName).VariantValue = VariantName.VariantValue;
                            variantNameModels.FirstOrDefault(a => a.VariantValue == temp).bgcolor = "#fff";
                        }
                        eee = getVariantsBySelectedValue(barkodlar, variantName, VariantName.VariantValue, count);




                    }
                    else if (variantNameModels.FirstOrDefault(a => a.VariantValue == VariantName.VariantValue).bgcolor != "#808080")
                    {
                        var varyants = productPopUp.Variants.SelectMany(x => x.VariantNames).Select(x => new { VariantName = x.VariantName, VariantValues = x.VariantValue }).Distinct();
                        string variantName = varyants.Where(x => x.VariantValues == VariantName.VariantValue).Select(y => y.VariantName).FirstOrDefault();


                        List<string> barkodlar = getVariantBarcodesByVariantValue(VariantName.VariantValue);
                        if (variantNameModels.Count < 1)
                        {
                            foreach (var varyant in varyants)
                            {
                                variantNameModels.Add(new VariantNameModel { VariantName = varyant.VariantName, VariantValue = varyant.VariantValues });
                            }
                        }

                        if (secimler.FirstOrDefault(a => a.VariantName == VariantName.VariantName) == null)
                        {
                            secimler.Add(new secimModel { VariantName = VariantName.VariantName, VariantValue = VariantName.VariantValue });
                        }
                        else if (secimler.FirstOrDefault(a => a.VariantName == VariantName.VariantName && a.VariantValue != VariantName.VariantValue) != null)
                        {
                            secimler.FirstOrDefault(a => a.VariantName == VariantName.VariantName && a.VariantValue != VariantName.VariantValue).VariantValue = VariantName.VariantValue;
                        }
                        else
                        {
                            secimler.Where(a => a.VariantName == VariantName.VariantName).Select(b => b.VariantValue = VariantName.VariantValue);
                        }

                        int index = secimler.IndexOf(secimler.Find(a => a.VariantName == VariantName.VariantName));
                        if (index < 1)
                        {
                            secimler = new List<secimModel>();
                            secimler.Add(new secimModel { VariantName = VariantName.VariantName, VariantValue = VariantName.VariantValue });

                        }
                        int count = secimler.Count;
                        if (katmanlar.Find(a => a.VariantName == VariantName.VariantName) == null)
                        {
                            katmanlar.Add(new variants { VariantName = VariantName.VariantName, katman = counter, isSelected = true, VariantValue = VariantName.VariantValue });

                            counter++;
                        }
                        else
                        {
                            string temp = katmanlar.FirstOrDefault(a => a.VariantName == VariantName.VariantName).VariantValue;


                            katmanlar.FirstOrDefault(a => a.VariantName == VariantName.VariantName).VariantValue = VariantName.VariantValue;
                            if (variantNameModels.FirstOrDefault(a => a.VariantValue == temp).bgcolor == "#22B07D")
                            {

                                variantNameModels.FirstOrDefault(a => a.VariantValue == temp).bgcolor = "#fff";

                            }
                        }
                        eee = getVariantsBySelectedValue(barkodlar, variantName, VariantName.VariantValue, count);


                    }


                }
                var model = eee.OrderBy(b => b.VariantValue).GroupBy(a => a.VariantName).OrderBy(c => c.Key); ;
                NewsItem.Clear();

                foreach (var item in model)
                {
                    ObservableCollection<VariantHeaderModel> h = new ObservableCollection<VariantHeaderModel>();
                    foreach (var item1 in item)
                    {
                        h.Add(new VariantHeaderModel { bgcolor = item1.bgcolor, VariantValue = item1.VariantValue, kisitlayan = item1.kisitlayan, isSelected = item1.isSelected });

                    }
                    var temp = h;
                    NewsItem.Add(new GroupedVariantHeaderModel { VariantName = item.Key, VariantValues = temp });

                }
            }   //katmanlar burda belli oluyor
        }
        public class secimModel
        {
            public string VariantName { get; set; }
            public string VariantValue { get; set; }

        }
        public class variants
        {
            public string VariantName { get; set; }
            public string VariantValue { get; set; }

            public int katman { get; set; }
            public bool isSelected { get; set; }
        }

        public List<VariantNameModel> variantNameModels = new List<VariantNameModel>();
        List<secimModel> secimler = new List<secimModel>();
        static List<variants> katmanlar = new List<variants>();
        private void onSelectVariant(object param)
        {

            VariantHeaderModel selectedVariant = (VariantHeaderModel)param;

            OnTappedSubCategory(selectedVariant.VariantValue);


        }
        int counter = 0;
        //int counterRandomVariant = 0;
        string sonsecim;
        private void OnTappedSubCategory(string value)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                if (variantNameModels.FirstOrDefault(a => a.VariantValue == value).bgcolor != "#808080")
                {
                    sonsecim = value;
                    var varyants = productPopUp.Variants.SelectMany(x => x.VariantNames).Select(x => new { VariantName = x.VariantName, VariantValues = x.VariantValue }).Distinct();

                    string variantName = varyants.Where(x => x.VariantValues == value).Select(y => y.VariantName).FirstOrDefault();
                    if (katmanlar.Find(a => a.VariantName == variantName) == null)
                    {
                        katmanlar.Add(new variants { VariantName = variantName, katman = counter, isSelected = true, VariantValue = value });
                        counter++;
                    }

                    else
                    {
                        string temp = katmanlar.FirstOrDefault(a => a.VariantName == variantName).VariantValue;


                        katmanlar.FirstOrDefault(a => a.VariantName == variantName).VariantValue = value;
                        variantNameModels.FirstOrDefault(a => a.VariantValue == temp).bgcolor = "#fff";
                    }
                    if (secimler.FirstOrDefault(a => a.VariantName == variantName) == null)
                    {
                        secimler.Add(new secimModel { VariantName = variantName, VariantValue = value });
                    }
                    else if (secimler.FirstOrDefault(a => a.VariantName == variantName && a.VariantValue != value) != null)
                    {
                        secimler.FirstOrDefault(a => a.VariantName == variantName && a.VariantValue != value).VariantValue = value;
                    }
                    List<string> barkodlar = getVariantBarcodesByVariantValue(value);
                    if (variantNameModels.Count < 1)
                    {
                        foreach (var varyant in varyants)
                        {
                            variantNameModels.Add(new VariantNameModel { VariantName = varyant.VariantName, VariantValue = varyant.VariantValues });
                        }
                    }
                    List<ProductVariantGroupModel> varyantlar = new List<ProductVariantGroupModel>();

                    foreach (var item in barkodlar)
                    {

                        varyantlar.Add(productPopUp.Variants.Where(v => v.Barcode == item).FirstOrDefault());
                    }

                    var secilebilecekler = varyantlar.SelectMany(x => x.VariantNames);

                    // int katman = katmanlar.FirstOrDefault(a => a.VariantName == variantName).katman;
                    //var k = katmanlar.FindAll(a => a.katman <= katman);
                    //ProductVariantGroupModel randomVariant = productPopUp.Variants.FirstOrDefault(a => a.VariantNames.FirstOrDefault(n => n.VariantValue == katmanlar.FirstOrDefault().VariantValue) != null);
                    tempKatman.AddRange(katmanlar);
                    ProductVariantGroupModel b = getVariantsByKatmans();
                    getRandomVariantPopUp(b.VariantNames.OrderBy(a => a.VariantName));
                }

            }
        }
        List<ProductVariantGroupModel> RandomVariant = null;

        List<variants> tempKatman = new List<variants>();
        ProductVariantGroupModel getVariantsByKatmans()
        {
            ProductVariantGroupModel p = null;
            bool flag1 = true;
            RandomVariant = productPopUp.Variants.FindAll(a => a.VariantNames.FirstOrDefault(c => c.VariantValue == sonsecim) != null);

            foreach (var item in RandomVariant)
            {
                item.VariantNames.OrderBy(a => a.VariantName);
                flag1 = true;
                foreach (var katman in katmanlar)
                {
                    if (item.VariantNames.FirstOrDefault(a => a.VariantValue == katman.VariantValue) == null)
                    {
                        flag1 = false;
                    }

                }
                if (flag1 == true)
                {
                    p = item;
                    break;
                }
            }


            if (p == null)
            {


                bool flagg = true;
                RandomVariant = productPopUp.Variants.FindAll(a => a.VariantNames.FirstOrDefault(c => c.VariantValue == sonsecim) != null);

                var x = RandomVariant.FindAll(a => a.VariantNames != null).SelectMany(b => b.VariantNames).OrderBy(c => c.VariantName);
                foreach (var counter in tempKatman)
                {
                    if (p != null)
                    {
                        break;
                    }
                    bool flag = false;
                    foreach (var item1 in tempKatman)//teker teker sonuncuyu silerek dene
                    {
                        if (flag == true)
                        {
                            var temp = katmanlar.LastOrDefault();
                            katmanlar.Remove(temp);
                            break;//katmanlara göre sec secebiliyorsa vardır
                        }
                        if (item1.VariantValue == sonsecim)
                        {
                            flag = true;
                        }


                    }

                    foreach (var item in RandomVariant)
                    {

                        item.VariantNames.OrderBy(a => a.VariantName);
                        flagg = true;
                        foreach (var katman in katmanlar)
                        {
                            if (item.VariantNames.FirstOrDefault(a => a.VariantValue == katman.VariantValue) == null)
                            {
                                flagg = false;
                            }

                        }
                        if (flagg == true)
                        {
                            p = item;
                            break;
                        }
                    }
                }
                //  p = RandomVariant.FirstOrDefault(a => a.VariantNames.FirstOrDefault(c => c.VariantValue == katmanlar.FirstOrDefault().VariantValue) != null);



            }
            RandomVariant = null;
            tempKatman = new List<variants>();
            return p;
        }
        List<string> getVariantBarcodesByVariantValue(string variantValue)
        {
            var BarcodeAndVariant = productPopUp.Variants.Select(x => new
            {
                Barcode = x.Barcode,
                Variant = x.VariantNames.FindAll(y => y.VariantValue == variantValue)

            });
            var BarcodeAndVariantt = BarcodeAndVariant.Where(x => x.Variant.Count > 0);
            List<string> barkodlar = new List<string>();
            foreach (var barcode in BarcodeAndVariantt)
            {
                barkodlar.Add(barcode.Barcode);
            }
            return barkodlar;
        }

        List<VariantNameModel> getVariantsBySelectedValue(List<string> variantBarcodes, string variantName, string variantValue, int count)
        {

            List<ProductVariantGroupModel> varyantlar = new List<ProductVariantGroupModel>();

            foreach (var item in variantBarcodes)
            {

                varyantlar.Add(productPopUp.Variants.Where(v => v.Barcode == item).FirstOrDefault());
            }
            List<ProductVariantGroupModel> varyants = new List<ProductVariantGroupModel>();

            bool flagg = true;
            varyantlar.OrderBy(a => a.VariantNames);
            foreach (var item in varyantlar)
            {
                flagg = true;
                foreach (var secim in secimler)
                {
                    if (item.VariantNames.FirstOrDefault(a => a.VariantValue == secim.VariantValue) == null)
                    {
                        flagg = false;
                    }

                }
                if (flagg == true)
                {
                    varyants.Add(item);

                }
            }




            RandomVariant = null;

            ////////////////////////////////////////////////////////////

            var secilebilecekler = varyants.SelectMany(x => x.VariantNames);

            foreach (var item in variantNameModels)
            {

                int gelenKatman = katmanlar.FirstOrDefault(a => a.VariantName == variantName).katman;
                int karsılaştırılacakKatman;
                if (katmanlar.FirstOrDefault(a => a.VariantName == item.VariantName) != null)
                {
                    karsılaştırılacakKatman = katmanlar.FirstOrDefault(a => a.VariantName == item.VariantName).katman;
                }
                else
                {
                    karsılaştırılacakKatman = 9999999;
                }
                if (gelenKatman < karsılaştırılacakKatman)
                {
                    if (secilebilecekler.FirstOrDefault(a => a.VariantValue == item.VariantValue) == null && item.VariantName != variantName)

                    {
                        item.bgcolor = "#808080";
                        item.kisitlayan = gelenKatman;
                    }
                    else
                    {
                        if (item.bgcolor != "#808080" || gelenKatman < karsılaştırılacakKatman)
                        {
                            if (item.bgcolor == "#808080" && gelenKatman == karsılaştırılacakKatman)
                            {

                            }
                            else
                            {
                                if (item.kisitlayan < gelenKatman)
                                {

                                }
                                else
                                {
                                    item.bgcolor = "#fff";

                                }

                            }
                        }




                    }
                }
                else
                {

                    if (gelenKatman == karsılaştırılacakKatman)
                    {
                        if (item.VariantValue == variantValue && item.VariantName == variantName)
                        {
                            item.bgcolor = "#22B07D";

                        }
                        else
                        {
                            if (item.bgcolor != "#808080" && item.bgcolor != "#22B07D")
                            {
                                if (item.bgcolor == "#808080")
                                {

                                }
                                else
                                {
                                    item.bgcolor = "#fff";

                                }

                            }

                        }
                    }
                    else
                    {
                        if (item.VariantValue == variantValue && item.VariantName == variantName)
                        {
                            item.bgcolor = "#22B07D";

                        }

                    }



                }



            }

            var emre = variantNameModels;
            return emre;

        }





        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }


}


    public abstract class ExtendedBindableObject : BindableObject
    {

        public void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = GetMemberInfo(property).Name;
            OnPropertyChanged(name);
        }

        private MemberInfo GetMemberInfo(Expression expression)
        {
            MemberExpression operand;
            LambdaExpression lambdaExpression = (LambdaExpression)expression;
            if (lambdaExpression.Body is UnaryExpression)
            {
                UnaryExpression body = (UnaryExpression)lambdaExpression.Body;
                operand = (MemberExpression)body.Operand;
            }
            else
            {
                operand = (MemberExpression)lambdaExpression.Body;
            }
            return operand.Member;
        }

    }

}



