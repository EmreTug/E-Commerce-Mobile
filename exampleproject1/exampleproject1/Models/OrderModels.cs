using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using static exampleproject1.Models.OrderLineModels;

namespace exampleproject1.Models
{
    public class GetOrderModel
    {

        [JsonProperty("PageNumber")]
        public int? Page { get; set; }
        [JsonProperty("PageSize")]
        public int? Size { get; set; }

    }
    public partial class GetOrderResponseModel
    {

        public long? Id { get; set; }


        public string OrderDate { get; set; }


        public string CustomerName { get; set; }


        public string CustomerAddress { get; set; }


        public string CustomerPhoneNumber { get; set; }


        public string CustomerMailAddress { get; set; }


        public string CustomerTaxAdministration { get; set; }


        public long? CustomerTaxId { get; set; }

        public ICollection<GetOrderLineResponseModel> OrderLine { get; set; }
        //    public List<OrderLine> OrderLine { get; set; } = new List<OrderLine>();
        //}
        //public class OrderLine
        //{
        //    public long Ahmet { get; set; }
        //}


        


    }

    public class TempOrderAddModel
    {
        public TempOrderAddModel()
        {
            OrderLine = new HashSet<TempOrderLineAddModel>();
        }

        public long AdminId { get; set; }


        public virtual ICollection<TempOrderLineAddModel> OrderLine { get; set; }
    }
    public class TempOrderLineAddModel
    {
        public long? ProductVariantGroupId { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string date { get; set; }
        public string barcode { get; set; }
     

    }
    //---------------------------------------------------------------------------------------
    
    //----------------------------------------------------------------------------------------
    public class TempOrderListModel
    {
        public TempOrderListModel()
        {
            OrderLine = new HashSet<TempOrderLineListModel>();
        }

        public long AdminId { get; set; }

        public virtual ICollection<TempOrderLineListModel> OrderLine { get; set; }
    }
    public class TempOrderLineListModel
    {

        public long? ProductVariantGroupId { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string date { get; set; }



    }
}
