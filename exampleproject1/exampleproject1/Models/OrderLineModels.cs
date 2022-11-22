using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using static exampleproject1.Models.ProductModels;
using static exampleproject1.Models.ProductVariantGroupModels;

namespace exampleproject1.Models
{
    public class OrderLineModels
    {
        public partial class GetOrderLineResponseModel
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("productVariantGroupId")]
            public long? ProductVariantGroupId { get; set; }

            [JsonProperty("orderId")]
            public long OrderId { get; set; }

            [JsonProperty("productId")]
            public long ProductId { get; set; }

            [JsonProperty("quantity")]
            public long Quantity { get; set; }

            [JsonProperty("unitPrice")]
            public long UnitPrice { get; set; }

            [JsonProperty("discountRate")]
            public long DiscountRate { get; set; }

            [JsonProperty("order")]
            public GetOrderResponseModel Order { get; set; }

            [JsonProperty("product")]
            public GetProductResponseModel Product { get; set; }

            [JsonProperty("productVariantGroup")]
            public GetProductVariantGroupResponseModel ProductVariantGroup { get; set; }
        }
    }


    public class OrderLineModel
    {

        public long Id { get; set; }
        public long? ProductVariantGroupId { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public float DiscountRate { get; set; }
        public GetOrderResponseModel Order { get; set; }
        public ProductListModel ProductModel { get; set; }
        public VariantNameModel ProductVariantGroup { get; set; }


    }



    public class OrderLineProductModel : INotifyPropertyChanged
    {  public long id { get; set; }
        public string image { get; set; }
        public long ProductId { get; set; }
        public long? ProductVariantGroupId { get; set; }
        public long OrderId { get; set; }
        public string Description { get; set; }
        public long? stock{get;set;}
        public string date { get; set; }
        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

    


        public string ProductName { get; set; }
     
        public double UnitPrice { get; set; }

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
 
        public float DiscountRate { get; set; }


        private long _quantity;
        public long Quantity
        {
            get
            {
                return _quantity;

            }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }


        



        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }


      


       

    }
    public class OrderAddModel
    {
        public OrderAddModel()
        {
            OrderLine = new List<OrderLineAddModel>();
        }

        public string OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerMailAddress { get; set; }
        public string CustomerTaxAdministration { get; set; }
        public long CustomerTaxId { get; set; }

        public virtual List<OrderLineAddModel> OrderLine { get; set; }
    }
    public class OrderLineAddModel
    {
        public long? ProductVariantGroupId { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double DiscountRate { get; set; }
      

    }
}
