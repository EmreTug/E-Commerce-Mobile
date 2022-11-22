using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static exampleproject1.Models.ProductModels;
using static exampleproject1.Models.VariantPictureModels;

namespace exampleproject1.Models
{
    public class PictureModels
    {
       

        public class GetPictureResponseModel
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("productId")]
            public long ProductId { get; set; }

            [JsonProperty("picture1")]
            public string Picture1 { get; set; }

            [JsonProperty("product")]
            public GetProductResponseModel Product { get; set; }

            [JsonProperty("variantPicture")]
            public List<GetVariantPictureResponseModel> VariantPicture { get; set; }
        }
    }
}
