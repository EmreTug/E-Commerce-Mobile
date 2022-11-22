using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static exampleproject1.Models.PictureModels;
using static exampleproject1.Models.ProductVariantGroupModels;

namespace exampleproject1.Models
{
    public class VariantPictureModels
    {
        public partial class GetVariantPictureResponseModel
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("pictureId")]
            public long PictureId { get; set; }

            [JsonProperty("productVariantGroupId")]
            public long ProductVariantGroupId { get; set; }

            [JsonProperty("picture")]
            public GetPictureResponseModel Picture { get; set; }

            [JsonProperty("productVariantGroup")]
            public GetProductVariantGroupResponseModel ProductVariantGroup { get; set; }
        }
    }
}
