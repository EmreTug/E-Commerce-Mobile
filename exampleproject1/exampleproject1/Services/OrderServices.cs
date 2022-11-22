using exampleproject1.Helpers;
using exampleproject1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace exampleproject1.Services
{
    public class OrderServices
    {
        public static async Task<List<GetOrderResponseModel>> OrderList(GetOrderModel getOrderModel)
        {


            HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
            using (var httpClient = new HttpClient(clientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), ProductServices.url+"api/Orders/GetAllOrder"))
                {
                    request.Version = HttpVersion.Version11;
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");

                    request.Content = new StringContent(JsonConvert.SerializeObject(getOrderModel));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string responseString = await response.Content.ReadAsStringAsync();
                            var responseModel = JsonConvert.DeserializeObject<List<GetOrderResponseModel>>(responseString);
                            return responseModel;
                        }

                    }
                }
            }
            return null;

        }

        public class ApiResultModel
        {
            public ApiResultModel()
            {
                ErrorMessages = new List<string>();
            }
            public bool Success { get; set; }
            public object item { get; set; }
            public List<string> ErrorMessages { get; set; }

        }
        public static async Task<bool> AddOrder(OrderAddModel setOrderModel)
        {
           
                HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
                using (var httpClient = new HttpClient(clientHandler))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), ProductServices.url+"api/Orders/PostOrder"))
                    {
                        request.Version = HttpVersion.Version11;
                     request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");

                    request.Content = new StringContent(JsonConvert.SerializeObject(setOrderModel));
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string responseString = await response.Content.ReadAsStringAsync();
                                var responseModel = JsonConvert.DeserializeObject<ApiResultModel>(responseString);
                                return true;
                            }

                        }
                    }
                }
                return false;
            
         

         

        }
        public static async Task<bool> AddTempOrder(TempOrderAddModel setTempOrderModel)
        {
             HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
                using (var httpClient = new HttpClient(clientHandler))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), ProductServices.url+ $"api/Orders/AddTempOrder?token={Settings.AccessToken}"))
                    {
                        request.Version = HttpVersion.Version11;
                     request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");

                    request.Content = new StringContent(JsonConvert.SerializeObject(setTempOrderModel));
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string responseString = await response.Content.ReadAsStringAsync();
                               // var responseModel = JsonConvert.DeserializeObject<List<GetOrderResponseModel>>(responseString);
                                return true;
                            }

                        }
                    }
                }
                return false;
         



        }


        public static async Task<TempOrderListModel> getTempOrder(string token)
        {
            HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
            using (var httpClient = new HttpClient(clientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), ProductServices.url + $"api/Orders/GetTempOrders?token={token}"))
                {
                    request.Version = HttpVersion.Version11;
                     request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");


                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string responseString = await response.Content.ReadAsStringAsync();
                            var responseModel = JsonConvert.DeserializeObject<TempOrderListModel>(responseString);
                            return responseModel;
                        }

                    }
                }
            }
            return null;

        }
        public static async Task<bool> updateTempOrder(TempOrderAddModel model)
        {
           
                HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
                using (var httpClient = new HttpClient(clientHandler))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("PUT"), ProductServices.url+$"api/Orders/updateTempOrder?Token={Settings.AccessToken}"))
                    {
                        request.Version = HttpVersion.Version11;
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");

                    request.Content = new StringContent(JsonConvert.SerializeObject(model));
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string responseString = await response.Content.ReadAsStringAsync();
                                // var responseModel = JsonConvert.DeserializeObject<List<GetOrderResponseModel>>(responseString);
                                return true;
                            }

                        }
                    }
                }
                return false;
            
     



        }

        public static async Task<bool> deleteTempOrder(long pid,long? pvgid)
        {
          
                HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
                using (var httpClient = new HttpClient(clientHandler))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("DELETE"), ProductServices.url + $"api/Orders/DeleteTempOrderLine?token={Settings.AccessToken}&pid={pid}&pvgid={pvgid}"))
                    {
                        request.Version = HttpVersion.Version11;
                     request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");


                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string responseString = await response.Content.ReadAsStringAsync();
                                // var responseModel = JsonConvert.DeserializeObject<List<GetOrderResponseModel>>(responseString);
                                return true;
                            }

                        }
                    }
                }
                return false;
       



        }

    }
}
