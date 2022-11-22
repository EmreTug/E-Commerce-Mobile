using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace exampleproject1.Models
{
    public class AdminModels
    {
        public partial class GetAdminResponseModel
        {
            public string UserName { get; set; }


            public string Password { get; set; }
        }

        public partial class TokenResponseModel
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("expires_in")]
            public long? ExpiresIn { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("scope")]
            public string Scope { get; set; }
        }
    }
}
