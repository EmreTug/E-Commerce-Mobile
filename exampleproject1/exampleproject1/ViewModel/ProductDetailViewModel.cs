using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using exampleproject1.Models;
using System.Linq;
using Xamarin.Forms;
using System.Windows.Input;
using exampleproject1.Services;
using System.Linq.Expressions;
using System.Reflection;
using exampleproject1.Helpers;
using static exampleproject1.Models.ProductModels;
using Xamarin.Essentials;

namespace exampleproject1.ViewModel
{
    public class ProductDetailViewModel

    {



        public ProductDetailViewModel()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
               App.Current.MainPage= new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                LoadProductDetail();
                selectProduct = new Command(onSelectProduct);
                selectVariant = new Command(onSelectedVariant);
                addBasket = new Command(onAddBasket);
                selectCategory = new Command(onSelectCategory);
            }




        }
        private void onSelectedVariant(object param)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                VariantHeaderModel selectedVariant = (VariantHeaderModel)param;


                OnTappedSubCategory(selectedVariant.VariantValue);
            }

        }



        

        private async void onSelectCategory(object param)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                StaticClass.pagination.Size = 0;
                StaticClass.pagination.Page = 0;

                CategoryModel selectedCategory = (CategoryModel)param;


                long CategoryId = selectedCategory.CategoryId;
                FilterModel tempFilterModel = new FilterModel();
                tempFilterModel.CategoryId = selectedCategory.CategoryId;
                tempFilterModel.Name = selectedCategory.CategoryName;
                tempFilterModel.IsSelected = true;



                StaticClass.Instance.IsBusy = true;
                StaticClass.AllProducts.Clear();
                StaticClass.checkProductLoad = 1;

                FilterModel temp = new FilterModel();

                foreach (var item in StaticClass.Filters.ToList())
                {
                    if (item.CategoryId == CategoryId)
                    {
                        item.IsSelected = true;
                        if (ProductViewModel.selectedF.Count == 0)
                        {
                            ProductViewModel.selectedF.Add(item.CategoryId);
                           
                        }
                        else
                        {
                            foreach (var item2 in ProductViewModel.selectedF)
                            {
                                if (item2 != CategoryId)
                                {
                                    ProductViewModel.selectedF.Add(item.CategoryId);
                                   
                                }
                            }
                        }

                        for (int i = 0; i < StaticClass.Filters.Count; i++)
                        {
                            if (StaticClass.Filters[i].CategoryId == CategoryId)
                            {
                                StaticClass.Filters.Insert(0, tempFilterModel);
                                StaticClass.Filters.RemoveAt(i + 1);
                            }
                        }



                    }
                    else
                    {
                        item.IsSelected = false;
                        ProductViewModel.selectedF.Remove(item.CategoryId);
                        temp = item;
                        StaticClass.Filters.Remove(item);
                        StaticClass.Filters.Add(item);

                    }


                }
               


                //ObservableCollection<ProductListModel> CategoriesAllProducts = await ProductServices.ProductByCategory(CategoryId);


                StaticClass.productId = CategoryId;
                StaticClass.pagination.Page = 1;
                StaticClass.pagination.Size = 21;
                ProductViewModel.check2 = true;

                ObservableCollection<ProductListModel> CategoriesAllProducts = await ProductServices.ProductByCategory(StaticClass.pagination, CategoryId);

                foreach (var item in CategoriesAllProducts)
                {
                    StaticClass.AllProducts.Add(item);
                }
                // = CategoriesAllProducts;
                StaticClass.Instance.IsBusy = false;


            }


        }

        ICommand selectVariant, addBasket, selectProduct, selectCategory;
  



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
        public ICommand SelectProduct
        {
            get
            {
                return selectProduct;
            }
            set
            {
                if (selectProduct == null)
                    return;
                selectProduct = value;
            }

        }
        public ICommand SelectCategory
        {
            get
            {
                return selectCategory;
            }

            set
            {
                if (selectCategory == null)
                    return;
                selectCategory = value;
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

        private ProductListModel _detailProducts;
        public ProductListModel DetailProducts
        {
            get
            {
                if (_detailProducts == null)
                {
                    _detailProducts = new ProductListModel();

                }
                return _detailProducts;
            }

            set
            {
                _detailProducts = value;
            }


        }
        private ProductListModel _tempDetailProducts;
        public ProductListModel TempDetailProducts
        {
            get
            {
                if (_tempDetailProducts == null)
                {
                    _tempDetailProducts = new ProductListModel();

                }
                return _tempDetailProducts;
            }

            set
            {
                _tempDetailProducts = value;
            }


        }

        public bool IsProductSelected { get; private set; }
        public long FindVariantId()
        {

            var variantKatmanList = getVariantsByKatmans();
            if (variantKatmanList == null)
            {
                variantKatmanList = randomVariant;
            }

            return variantKatmanList.Id;

        }

        private async void AddProductBasket()
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





                orderLineModel.ProductName = DetailProducts.Name;
                orderLineModel.UnitPrice = DetailProducts.Price;
                orderLineModel.ProductId = DetailProducts.Id;
                orderLineModel.stock = DetailProducts.Stock;
                orderLineModel.image = DetailProducts.Images.FirstOrDefault();
                orderLineModel.Description = ProductViewModel.DetailProduct.Description.ToUpper();
                orderLineModel.TotalPrice = DetailProducts.Price;
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
                    ProductViewModel.TempOrderlineList = StaticClass.OrderLineList;
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
                    ProductViewModel.TempOrderlineList = StaticClass.OrderLineList;


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

        //emre
        private async void onAddBasket(object param)
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

                long VariantId = FindVariantId();

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
                            ProductVariantGroupId = item.ProductVariantGroupId

                        });


                    }

                    model.OrderLine = moddel;
                    ProductViewModel.TempOrderlineList = StaticClass.OrderLineList;
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
                    ProductViewModel.TempOrderlineList = StaticClass.OrderLineList;


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

       



        public List<CategoryModel> breadCump { get; set; }
        private  void LoadProductDetail()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                DetailProducts = ProductViewModel.DetailProduct;

            }


      


        }
        private void onSelectProduct()
        {

            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                productPopUp = DetailProducts;
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
                    var variants = productPopUp.Variants.SelectMany(x => x.VariantNames).Select(x => new { VariantName = x.VariantName, VariantValues = x.VariantValue, isSelected = true, bgcolor = "#fff" }).Distinct();
                    List<VariantNameModel> variantNameModel = new List<VariantNameModel>();

                    foreach (var variant in variants)
                    {
                        variantNameModel.Add(new VariantNameModel { VariantName = variant.VariantName, VariantValue = variant.VariantValues });
                    }
                    // NewsItem= variantNameModel.OrderBy(b => b.VariantValue).GroupBy(a => a.VariantName).OrderBy(c => c.Key);

                    randomVariant = productPopUp.Variants.FirstOrDefault(a => a.VariantNames != null);
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

        public static ProductListModel productPopUp;
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
            }
            //katmanlar burda belli oluyor
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
        { ProductVariantGroupModel p = null;
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
               
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
            }
            RandomVariant = null;
            tempKatman = new List<variants>();
            return p;

        }
        List<string> getVariantBarcodesByVariantValue(string variantValue)
        {List<string> barkodlar = new List<string>();
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.None)
            {
                App.Current.MainPage = new NavigationPage(new NoInternetConnectionPage());

            }
            else
            {
                var BarcodeAndVariant = productPopUp.Variants.Select(x => new
                {
                    Barcode = x.Barcode,
                    Variant = x.VariantNames.FindAll(y => y.VariantValue == variantValue)

                });
                var BarcodeAndVariantt = BarcodeAndVariant.Where(x => x.Variant.Count > 0);
                
                foreach (var barcode in BarcodeAndVariantt)
                {
                    barkodlar.Add(barcode.Barcode);
                }
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
    }



}