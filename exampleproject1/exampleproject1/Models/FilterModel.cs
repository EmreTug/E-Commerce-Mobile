using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace exampleproject1.Models
{
    public class FilterModel : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public long CategoryId { get; set; }

        public bool _isSelected { get; set; }
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged("Image"); OnPropertyChanged("IsSelected"); }
        }

        public string _Image { get; set; }
        public string Image
        {
            get
            {
                if (IsSelected)
                {
                    return "icon_checked.png";
                }
                else
                {
                    return "icon_unchecked.png";
                }
            }
            set
            {
                _Image = value; OnPropertyChanged("Image");
            }
        }



        public ObservableCollection<FilterModel> GetFilters()
        {
            return new ObservableCollection<FilterModel>
            {
                new FilterModel { Name = "T-Shirt", IsSelected = false},
                new FilterModel { Name = "Shoes", IsSelected = false },
                new FilterModel { Name = "Jackets", IsSelected = false},
                new FilterModel { Name = "Jeans", IsSelected = false },
                new FilterModel { Name = "Drees", IsSelected = false},
                new FilterModel { Name = "Sneakers", IsSelected = false }


            };
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}