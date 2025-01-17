﻿using StockProductTracking.Core;
using StockProductTracking.MVVM.Model;
using System.ComponentModel;

namespace StockProductTracking.MVVM.ViewModel
{
    internal abstract class OrderViewModelBase : ObservableObject, IDataErrorInfo
    {
        private Product currentProduct = new Product();
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string OrderProductTitle
        {
            get { return currentProduct.ProductTitle; }
            set 
            { 
                currentProduct.ProductTitle = value; 
                IsEnable = !string.IsNullOrEmpty(value);
            }
        }
        public string OrderProductPrice
        {
            get { return currentProduct.ProductPrice; }
            set { currentProduct.ProductPrice = value; }
        }
        public int OrderProductCount { get; set; }
        public bool OrderStatus { get; set; }

        private bool _IsEnable = false;
        public bool IsEnable
        {
            get
            {
                return _IsEnable;
            }

            set
            {
                _IsEnable = value;
                OnPropertyChanged();
            }
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;
                if (columnName == "OrderProductCount")
                {
                   if (!(this.OrderProductCount > 0))
                        result = "En az 1 adet satın almalısınız.";
                }
                if (columnName == "CustomerId")
                {
                    IsEnable = CustomerId != 0;
                }
                return result;
            }
        }

    }
}
