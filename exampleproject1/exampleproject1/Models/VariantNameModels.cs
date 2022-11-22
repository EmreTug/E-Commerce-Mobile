using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static exampleproject1.Models.ProductVariantModels;
using static exampleproject1.Models.VariantValueModels;

namespace exampleproject1.Models
{
    public class VariantNameModels
    {
        public partial class GetVariantNameResponseModel
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("variantName1")]
            public string VariantName1 { get; set; }

            [JsonProperty("productVariant")]
            public ICollection<GetProductVariantResponseModel> ProductVariant { get; set; }

            [JsonProperty("variantValue")]
            public ICollection<GetVariantValueResponseModel> VariantValue { get; set; }
        }
    }
}
