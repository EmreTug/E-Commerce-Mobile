using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static exampleproject1.Models.ProductVariantModels;
using static exampleproject1.Models.VariantNameModels;

namespace exampleproject1.Models
{
    public class VariantValueModels
    {
        public partial class GetVariantValueResponseModel
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("variantNameId")]
            public long VariantNameId { get; set; }

            [JsonProperty("variantName")]
            public GetVariantNameResponseModel VariantName { get; set; }

            [JsonProperty("productVariant")]
            public ICollection<GetProductVariantResponseModel> ProductVariant { get; set; }
        }
    }
}
