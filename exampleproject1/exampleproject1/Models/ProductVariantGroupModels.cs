using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static exampleproject1.Models.ProductModels;
using static exampleproject1.Models.ProductVariantModels;
using static exampleproject1.Models.VariantPictureModels;

namespace exampleproject1.Models
{
    public class ProductVariantGroupModels
    {
        public partial class GetProductVariantGroupResponseModel
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("productId")]
            public long ProductId { get; set; }

            [JsonProperty("price")]
            public long Price { get; set; }

            [JsonProperty("barcode")]
            public string Barcode { get; set; }

            [JsonProperty("stock")]
            public long Stock { get; set; }

            [JsonProperty("product")]
            public GetProductResponseModel Product { get; set; }

            [JsonProperty("orderLine")]
            public ICollection<GetProductResponseModel> OrderLine { get; set; }

            [JsonProperty("productVariant")]
            public ICollection<GetProductVariantResponseModel> ProductVariant { get; set; }

            [JsonProperty("variantPicture")]
            public ICollection<GetVariantPictureResponseModel> VariantPicture { get; set; }
        }
    }
}
