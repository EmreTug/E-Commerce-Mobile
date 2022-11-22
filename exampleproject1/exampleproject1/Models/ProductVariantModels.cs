using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static exampleproject1.Models.ProductVariantGroupModels;
using static exampleproject1.Models.VariantNameModels;
using static exampleproject1.Models.VariantValueModels;

namespace exampleproject1.Models
{
    public class ProductVariantModels
    {

        public partial class GetProductVariantResponseModel
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("productVariantGroupId")]
            public long ProductVariantGroupId { get; set; }

            [JsonProperty("variantNameId")]
            public long VariantNameId { get; set; }

            [JsonProperty("variantValueId")]
            public long VariantValueId { get; set; }

            [JsonProperty("productVariantGroup")]
            public GetProductVariantGroupResponseModel ProductVariantGroup { get; set; }

            [JsonProperty("variantName")]
            public GetVariantNameResponseModel VariantName { get; set; }

            [JsonProperty("variantValue")]
            public GetVariantValueResponseModel VariantValue { get; set; }
        }
    }
}
