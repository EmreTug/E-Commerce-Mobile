using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static exampleproject1.Models.PictureModels;

namespace exampleproject1.Services
{
    public class PictureService
    {
        public static async Task<List<GetPictureResponseModel>> PictureByProductId(long id)
        {


            HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
            using (var httpClient = new HttpClient(clientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"http://192.168.1.191:45455/api/Products/getPictureByProductID?id=10002"))
                {
                    request.Version = HttpVersion.Version11;

                    using (var response = await httpClient.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string responseString = await response.Content.ReadAsStringAsync();

                            var responseModel = JsonConvert.DeserializeObject<List<GetPictureResponseModel>>(responseString);
                            return responseModel;
                        }

                    }
                }
            }
            return null;

        }
    }
}
