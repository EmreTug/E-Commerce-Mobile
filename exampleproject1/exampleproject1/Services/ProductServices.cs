using exampleproject1.Helpers;
using exampleproject1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static exampleproject1.Models.ProductModels;

namespace exampleproject1.Services
{
    class ProductServices
    {
        public static string url = "http://192.168.1.191:45455/";
        public static async Task<List<ProductListModel>> ProductList(GetProductModel getProductModel)
        {


            HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
            using (var httpClient = new HttpClient(clientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url+ $"api/Products/GetAllProduct?PageNumber={getProductModel.Page}&PageSize={getProductModel.Size}"))
                {
                    request.Version = HttpVersion.Version11;
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");
                    request.Content = new StringContent(JsonConvert.SerializeObject(getProductModel));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            
                            string responseString = await response.Content.ReadAsStringAsync();
                            List<ProductListModel> responseModel = JsonConvert.DeserializeObject<List<ProductListModel>>(responseString);
                            
                            foreach (var item in responseModel)
                            {
                                item.Category.BreadCumb.Reverse();
                            }
                            return responseModel;
                        }

                    }
                }
            }
            return null;
        }

        public static async Task<ObservableCollection<CategoryModel>> AllCategory()
        {


            HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
            using (var httpClient = new HttpClient(clientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url + $"api/Products/GetAllCategory"))
                {
                    request.Version = HttpVersion.Version11;

                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");

                    using (var response = await httpClient.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {

                            string responseString = await response.Content.ReadAsStringAsync();
                        
                            ObservableCollection<CategoryModel> responseModel = JsonConvert.DeserializeObject<ObservableCollection<CategoryModel>>(responseString);

                           
                            return responseModel;
                        }

                    }
                }
            }
            return null;

        }



        public static async Task<List<ProductListModel>> ProductByCategory(int id)
        {
            HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
            using (var httpClient = new HttpClient(clientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url + $"/GetProductByCategory?name={id}"))
                {
                    request.Version = HttpVersion.Version11;
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");


                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {

                            string responseString = await response.Content.ReadAsStringAsync();
                            var responseModel = JsonConvert.DeserializeObject<List<ProductListModel>>(responseString);
                            return responseModel;
                        }

                    }
                }
            }
            return null;

        }



       

        public static async Task<ProductListModel> ProductById(long id)
        {


            HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
            using (var httpClient = new HttpClient(clientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url+$"api/Products/GetProductById?id={id}"))
                {
                    request.Version = HttpVersion.Version11;
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");


                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {

                            string responseString = await response.Content.ReadAsStringAsync();
                            var responseModel = JsonConvert.DeserializeObject<ProductListModel>(responseString);
                            return responseModel;
                        }

                    }
                }
            }
            return null;

        }
        public static async Task<ProductListModel> ProductByBarcode(string barcode)
        {


            HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
            using (var httpClient = new HttpClient(clientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url + $"api/Products/getProductByBarcode?barcode={barcode}&token={Settings.AccessToken}"))
                {
                    request.Version = HttpVersion.Version11;
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");


                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {

                            string responseString = await response.Content.ReadAsStringAsync();
                            var responseModel = JsonConvert.DeserializeObject<ProductListModel>(responseString);
                            return responseModel;
                        }

                    }
                }
            }
            return null;

        }
        public static async Task<ProductListModel> VariantByBarcode(string barcode)
        {


            HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
            using (var httpClient = new HttpClient(clientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url + $"api/Products/getVariantByBarcode?barcode={barcode}&token={Settings.AccessToken}"))
                {
                    request.Version = HttpVersion.Version11;
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");


                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {

                            string responseString = await response.Content.ReadAsStringAsync();
                            var responseModel = JsonConvert.DeserializeObject<ProductListModel>(responseString);
                            return responseModel;
                        }

                    }
                }
            }
            return null;

        }





        //public static async Task<ObservableCollection<ProductListModel>> ProductByCategory(long id)
        //{


        //    HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
        //    using (var httpClient = new HttpClient(clientHandler))
        //    {
        //        using (var request = new HttpRequestMessage(new HttpMethod("GET"), url + $"api/Products/GetProductByCategory?id={id}"))
        //        {
        //            request.Version = HttpVersion.Version11;

        //           request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");


        //            using (var response = await httpClient.SendAsync(request))
        //            {
        //                if (response.IsSuccessStatusCode)
        //                {

        //                    string responseString = await response.Content.ReadAsStringAsync();
        //                    var responseModel = JsonConvert.DeserializeObject<ObservableCollection<ProductListModel>>(responseString);
        //                    return responseModel;
        //                }

        //            }
        //        }
        //    }
        //    return null;

        //}


        public static async Task<ObservableCollection<ProductListModel>> ProductByCategory(GetProductModel getProductModel, long id)
        {


            HttpClientHandler clientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; } };
            using (var httpClient = new HttpClient(clientHandler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url + $"api/Products/GetProductByCategory?id={id}&PageNumber={getProductModel.Page}&PageSize={getProductModel.Size}"))
                {
                    request.Version = HttpVersion.Version11;

                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {Settings.AccessToken}");
                    request.Content = new StringContent(JsonConvert.SerializeObject(getProductModel));


                    using (var response = await httpClient.SendAsync(request).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {

                            string responseString = await response.Content.ReadAsStringAsync();
                            ObservableCollection<ProductListModel> responseModel = JsonConvert.DeserializeObject<ObservableCollection<ProductListModel>>(responseString);
                            foreach (var item in responseModel)
                            {
                                item.Category.BreadCumb.Reverse();
                            }
                            return responseModel;
                        }

                    }
                }
            }
            return null;

        }


       
    }
}




