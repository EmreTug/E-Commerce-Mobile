using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using exampleproject1.Models;
using exampleproject1.Services;
using static exampleproject1.Models.ProductModels;

namespace exampleproject1.ViewModel
{
    public class MainPageViewModel 
    {

        public List<GetProductResponseModel> Products { get; set; }
        public List<GetOrderModel> Orders { get; set; }
        public MainPageViewModel()
        {
        
          
        }
       

    }



}



/*public MainPageViewModel()
        {
            Products = GetProducts();
        }

        private ObservableCollection<ProductModels> products;
        public ObservableCollection<ProductModels> Products
        {
            get { return products; }
            set
            {
                products = value;
                OnPropertyChanged();
            }
        }

        private ProductModels selectedProduct;
        public ProductModels SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<ProductModels> GetProducts()
        {
            return new ObservableCollection<ProductModels>
            {
                new ProductModels { Name = "CLASSIC", Image = "filter.png", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Bibendum est ultricies"},
                new ProductModels { Name = "DOUBLE", Image = "filter.png", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Bibendum est ultricies"},
                new ProductModels { Name = "SHARK", Image = "filter.png", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Bibendum est ultricies"},
                new ProductModels { Name = "CHICKEN", Image = "filter.png", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Bibendum est ultricies"},
                new ProductModels { Name = "MEAT",Image = "filter.png", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Bibendum est ultricies"},
                new ProductModels { Name = "BBQ", Image = "filter.png", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Bibendum est ultricies"}
            };
        }

*/