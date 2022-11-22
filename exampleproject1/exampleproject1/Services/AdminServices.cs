using exampleproject1.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static exampleproject1.Models.AdminModels;

namespace exampleproject1.Services
{
    public class AdminServices
    {
        public static  async Task<String> CreateToken()
        {
            HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
            using (var httpClient = new HttpClient(clientHandler))
            {

                using (var request = new HttpRequestMessage(HttpMethod.Post, ProductServices.url + "connect/token"))
                {
                    request.Version = HttpVersion.Version11;
                    var contentList = new List<string>();
                    contentList.Add($"client_id={Uri.EscapeDataString("testclient")}");
                    contentList.Add($"client_secret={Uri.EscapeDataString("testsecret")}");
                    contentList.Add($"grant_type={Uri.EscapeDataString("client_credentials")}");
                    contentList.Add($"scope={Uri.EscapeDataString("testapis")}");
                    request.Content = new StringContent(string.Join("&", contentList));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");  
                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string responseString = await response.Content.ReadAsStringAsync();
                            var responseModel = JsonConvert.DeserializeObject<TokenResponseModel>(responseString);
                            return responseModel.AccessToken;
                        }

                    }
                }
            }
            return "";

        }
       

        public static async Task<string> LoginCheckAsync(string username,string password)
        {
            HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
            using (var httpClient = new HttpClient(clientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), ProductServices.url+$"api/VariantNames/getAdminByUsername?username={username}&password={password}"))
                {
                    request.Version = HttpVersion.Version11;



                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string responseString = await response.Content.ReadAsStringAsync();
                            
                            return responseString;
                        }

                    }
                }
            }
            return null;
 
        }



        public static async Task<string> updateToken(string lastToken, string newToken)
        {
            HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
            using (var httpClient = new HttpClient(clientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("PUT"), ProductServices.url + $"api/VariantNames/updateAdminToken?lastToken={lastToken}&newToken={newToken}"))
                {
                    request.Version = HttpVersion.Version11;



                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string responseString = await response.Content.ReadAsStringAsync();

                            return responseString;
                        }

                    }
                }
            }
            return null;

        }
    }
}
