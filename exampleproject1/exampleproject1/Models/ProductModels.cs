using exampleproject1.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using static exampleproject1.Models.OrderLineModels;
using static exampleproject1.Models.PictureModels;
using static exampleproject1.Models.ProductVariantGroupModels;

namespace exampleproject1.Models
{
    //hamiyet
    //SELECTED PRODUCT
    public static class BarcodeScanner
    {

        //public static string ProductBarcode { get; set; }
        //public static List<SelectedProductVariant> selectedVariants = new List<SelectedProductVariant>();

        public static bool isScanned = false;
        public static List<ProductListModel> CurrentAllProducts { get; set; }
        public static string Barcode { get; set; }
    }


    //SELECTED PRODUCT VARIANT
    public class SelectedProductVariant
    {
        public string ProductBarcode { get; set; }
        public string ProductVariantGroup { get; set; }
        public string ProductVariantValue { get; set; }
    }




    //PRODUCT LIST MODEL
    public class ProductListModel
    {
        public long Id { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Stock { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public List<string> Images { get; set; }
        public string firstImage { get { return Images.FirstOrDefault(); } }
        public List<ProductVariantGroupModel> Variants { get; set; }
        public CategoryModel Category { get; set; }


        public ProductListModel()
        {

            Images = new List<string>();
            Variants = new List<ProductVariantGroupModel>();

        }


    }







    public class ProductVariantGroupModel

    {
        public ProductVariantGroupModel()
        {
            VariantNames = new List<VariantNameModel>();
        }
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Barcode { get; set; }
        public double? Price { get; set; }
        public long? Stock { get; set; }
        public List<string> Images { get; set; }
        public List<VariantNameModel> VariantNames { get; set; }

    }
    public class VariantNameModel
    {
        public string VariantValue { get; set; }
        public string VariantName { get; set; }
        public string bgcolor { get; set; }
        public bool isSelected { get; set; }
        public int kisitlayan = 999999;
    }




    public class VariantHeaderModel : INotifyPropertyChanged
    {

        public bool isSelected { get; set; }
        public int kisitlayan = 999999;
        public string VariantValue { get; set; }

        private string _bgcolor;
        public string bgcolor
        {
            get
            {
                return _bgcolor;

            }
            set
            {
                _bgcolor = value;
                OnPropertyChanged(bgcolor);
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

    }

    public class GroupedVariantHeaderModel : INotifyPropertyChanged
    {
        private string _variantName { get; set; }
        public string VariantName
        {
            get
            {
                return _variantName;

            }
            set
            {
                _variantName = value;
                OnPropertyChanged(VariantName);
            }
        }
        public ObservableCollection<VariantHeaderModel> VariantValues { get; set; } = new ObservableCollection<VariantHeaderModel>();
        public event PropertyChangedEventHandler PropertyChanged;


        void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

    }










    public class CategoryModel
    {

        public CategoryModel()
        {

            BreadCumb = new List<CategoryModel>();
        }
        public List<CategoryModel> BreadCumb { get; set; }
        public string CategoryName { get; set; }
        public long CategoryId { get; set; }


    }

    public class ProductModels
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public long Stock { get; set; }
        public float Price { get; set; }




        //GET PRODUCT MODEL
        public class GetProductModel
        {

            [JsonProperty("PageNumber")]
            public int? Page { get; set; } = 0;
            [JsonProperty("PageSize")]
            public int? Size { get; set; } = 21;

        }




        //GET PRODUCT RESPONSE MODEL
        public class GetProductResponseModel
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("barcode")]
            public string Barcode { get; set; }

            [JsonProperty("stock")]
            public long Stock { get; set; }

            [JsonProperty("price")]
            public long Price { get; set; }

            [JsonProperty("orderLine")]
            public ICollection<GetOrderLineResponseModel> OrderLine { get; set; }

            [JsonProperty("picture")]
            public ICollection<GetPictureResponseModel> Picture { get; set; }

            [JsonProperty("productVariantGroup")]
            public ICollection<GetProductVariantGroupResponseModel> ProductVariantGroup { get; set; }


            //public long? Id { get; set; }


            //public string Name { get; set; }


            //public string Description { get; set; }


            //public string Barcode { get; set; }

            //public long Stock { get; set; }

        }



    }
}
